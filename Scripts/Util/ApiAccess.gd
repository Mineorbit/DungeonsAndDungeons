extends Node


var auth_token = ""


var level_list
signal level_download_finished

func get_api_url():
	return Options.settings.api_url



func prepare_api():
	fetch_api_data()
	login("test","test")
	
#func _process(delta):
#	if Input.is_action_just_pressed("Connect"):
#		upload_level("test")





func download_level(ulid,local = false):
	print("Downloading Level with ULID: "+str(ulid))
	# Create an HTTP request node and connect its completion signal.
	var http_request = HTTPRequest.new()
	add_child(http_request)
	http_request.request_completed.connect(
		func(a,b,c,d):
			level_download_http_request_completed(a,b,c,d,local)
	)
	http_request.download_file = level_zip_file_path("download")
	# Perform the HTTP request. The URL below returns a PNG image as of writing.
	var error = http_request.request(str(get_api_url())+"level/download?proto_resp=false&ulid="+str(ulid))
	print("Link: "+str(get_api_url()+"level/download?proto_resp=false&ulid="+str(ulid)))
	if error != OK:
		push_error("An error occurred in the HTTP request.")



# Called when the HTTP request is completed.
func level_download_http_request_completed(result, _response_code, _headers, _body,local):
	if result != OK:
		push_error("Download Failed")
	print("Finished Downloading Level")
	decompress_level("download",local)
	level_download_finished.emit()



func fetch_level_list():
	# Create an HTTP request node and connect its completion signal.
	print(str(Constants.id)+" Started fetching Level List")
	var http_request = HTTPRequest.new()
	add_child(http_request)
	http_request.request_completed.connect(level_list_http_request_completed)
	# Perform a GET request. The URL below returns JSON as of writing.
	var error = http_request.request(str(get_api_url())+"level/all?proto_resp=f")
	if error != OK:
		push_error("An error occurred in the HTTP request.")
	# Perform a POST request. The URL below returns JSON as of writing.
	# Note: Don't make simultaneous requests using a single HTTPRequest node.
	# The snippet below is provided for reference only.

signal levels_fetched(list)

# Called when the HTTP request is completed.
func level_list_http_request_completed(_result, _response_code, _headers, body):
	var json_object = JSON.new()
	json_object.parse(body.get_string_from_utf8())
	level_list = json_object.data["levels"]
	# Will print the user agent string used by the HTTPRequest node (as recognized by httpbin.org).
	levels_fetched.emit(level_list)


# only for windows now
func compress_level(levelname,local = true):
	var subpath = "localLevels"
	if not local:
		subpath = "downloadLevels"
	var path = ProjectSettings.globalize_path("user://level/"+str(subpath)+"/"+str(levelname))
	var result = ProjectSettings.globalize_path("user://level/"+str(levelname)+".zip")
	OS.execute("powershell.exe",["Compress-Archive",path,result])
	
# only for windows now
func decompress_level(levelname,local = false):
	var subpath = "downloadLevels"
	if local:
		subpath = "localLevels"
	var path = ProjectSettings.globalize_path("user://level/"+str(levelname)+".zip")
	var result = ProjectSettings.globalize_path("user://level/"+subpath)
	print("Decompressing "+str(path)+" on "+str(OS.get_name()))
	if OS.get_name() == "Windows":
		OS.execute("powershell.exe",["Expand-Archive",path,result,"-Force"])
	else:
		OS.execute("unzip",[path,"-d "+str(result)])


	

func level_zip_file_path(levelname):
	var path = ProjectSettings.globalize_path("user://level/"+str(levelname)+".zip")
	return path

func upload_level(levelname,publiclevelname):
	print("Uploading "+str(levelname)+" as "+str(publiclevelname))
	compress_level(levelname)
	var http_request = HTTPRequest.new()
	add_child(http_request)
	http_request.request_completed.connect(upload_http_request_completed)
	
	
	var file = FileAccess.open(level_zip_file_path(levelname), FileAccess.READ)
	var file_content = file.get_buffer(file.get_length())
	
	var tn_file_name = "icon.png"
	var tnfile = FileAccess.open('res://%s' % tn_file_name, FileAccess.READ)
	var thumbnail_content = tnfile.get_buffer(tnfile.get_length())
	var body = PackedByteArray()
	body.append_array("\r\n--BodyBoundaryHere\r\n".to_utf8_buffer())
	body.append_array(("Content-Disposition: form-data; name=\"levelFiles\"; filename=\"%s\"\r\n" % levelname).to_utf8_buffer())
	body.append_array("Content-Type: application/zip\r\n\r\n".to_utf8_buffer())
	body.append_array(file_content)
	body.append_array("\r\n--BodyBoundaryHere\r\n".to_utf8_buffer())
	body.append_array(("Content-Disposition: form-data; name=\"thumbnail\"; filename=\"%s\"\r\n" % tn_file_name).to_utf8_buffer())
	body.append_array("Content-Type: image/png\r\n\r\n".to_utf8_buffer())
	body.append_array(thumbnail_content)
	body.append_array("\r\n--BodyBoundaryHere--\r\n".to_utf8_buffer())
	
	var headers = [
		"Authorization: Bearer "+str(auth_token),
	"Content-Length: " + str(body.size()),
	"Content-Type: multipart/form-data; boundary=\"BodyBoundaryHere\""
	]
	var description = "This is a level".uri_encode()
	# Perform the HTTP request. The URL below returns a PNG image as of writing.
	var public_level_name = publiclevelname.uri_encode()
	var request_url = str(get_api_url())+"level/?proto_resp=false&name="+str(public_level_name)+"&description="+str(description)+"&r=t&g=t&b=t&y=t"
	var error = http_request.request_raw(request_url, headers, true, HTTPClient.METHOD_POST, body)
	if error != OK:
		push_error("An error occurred in the HTTP request.")




func upload_http_request_completed(_result, _response_code, _headers, body):
	var json_object: JSON = JSON.new()
	json_object.parse(body.get_string_from_utf8())

func login(username,password):
	# Create an HTTP request node and connect its completion signal.
	var http_request = HTTPRequest.new()
	add_child(http_request)
	
	http_request.request_completed.connect(login_http_request_completed)
	var body = "grant_type=&username="+str(username)+"&password="+str(password)+"&scope=&client_id=&client_secret="
	# Perform the HTTP request. The URL below returns a PNG image as of writing.
	var error = http_request.request(str(get_api_url())+"auth/token", ["Content-Type: application/x-www-form-urlencoded"], true, HTTPClient.METHOD_POST, body)
	if error != OK:
		push_error("An error occurred in the HTTP request.")


func get_auth_header():
	return ["Authorization: Bearer "+str(auth_token)]

func login_http_request_completed(_result, _response_code, _headers, body):
	var json_object: JSON = JSON.new()
	json_object.parse(body.get_string_from_utf8())
	if json_object.data == null:
		print("Could not parse JSON")
		return
	if not json_object.data.has("access_token"):
		return
	auth_token = json_object.data["access_token"]
	print("Login Success: "+str(auth_token))
	


func download_thumbnail(ulid):
	# Create an HTTP request node and connect its completion signal.
	var http_request = HTTPRequest.new()
	add_child(http_request)
	http_request.request_completed.connect(thumbnail_http_request_completed)
	# Perform the HTTP request. The URL below returns a PNG image as of writing.
	var error = http_request.request(str(get_api_url())+"level/pic?proto_resp=false&ulid="+str(ulid))
	if error != OK:
		push_error("An error occurred in the HTTP request.")


func fetch_api_data():
	# Create an HTTP request node and connect its completion signal.
	var http_request = HTTPRequest.new()
	add_child(http_request)
	http_request.request_completed.connect(data_http_request_completed)
	# Perform a GET request. The URL below returns JSON as of writing.
	var error = http_request.request(str(get_api_url())+"")
	if error != OK:
		push_error("An error occurred in the HTTP request.")
	# Perform a POST request. The URL below returns JSON as of writing.
	# Note: Don't make simultaneous requests using a single HTTPRequest node.
	# The snippet below is provided for reference only.


# Called when the HTTP request is completed.
func data_http_request_completed(_result, _response_code, _headers, body):
	var json_object = JSON.new()
	json_object.parse(body.get_string_from_utf8())
	# Will print the user agent string used by the HTTPRequest node (as recognized by httpbin.org).
	#print(json_object.data["message"])



# Called when the HTTP request is completed.
func thumbnail_http_request_completed(_result, _response_code, _headers, body):
	var image = Image.new()
	var error = image.load_png_from_buffer(body)
	if error != OK:
		push_error("Couldn't load the image.")
	var texture = ImageTexture.create_from_image(image)
	# Display the image in a TextureRect node.
	var texture_rect = TextureRect.new()
	add_child(texture_rect)
	texture_rect.texture = texture
