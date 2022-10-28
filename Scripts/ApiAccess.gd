extends Node


var url = "https://mstillger.de/api/"
var auth_token = ""


var level_list


# Called when the node enters the scene tree for the first time.
func _ready():
	fetch_api_data()
	login("test","test")
	#download_level(6)

#func _process(delta):
#	if Input.is_action_just_pressed("Connect"):
#		upload_level("test")




signal level_download_finished

func download_level(ulid):
	print("Downloading Level with ULID: "+str(ulid))
	# Create an HTTP request node and connect its completion signal.
	var http_request = HTTPRequest.new()
	add_child(http_request)
	http_request.request_completed.connect(level_download_http_request_completed)
	http_request.download_file = level_zip_file_path("download")
	# Perform the HTTP request. The URL below returns a PNG image as of writing.
	var error = http_request.request(str(url)+"level/download?proto_resp=false&ulid="+str(ulid))
	print("Link: "+str(str(url)+"level/download?proto_resp=false&ulid="+str(ulid)))
	if error != OK:
		push_error("An error occurred in the HTTP request.")



# Called when the HTTP request is completed.
func level_download_http_request_completed(result, _response_code, _headers, _body):
	if result != OK:
		push_error("Download Failed")
	print("Finished Downloading Level")
	decompress_level("download")
	level_download_finished.emit()



func fetch_level_list():
	# Create an HTTP request node and connect its completion signal.
	var http_request = HTTPRequest.new()
	add_child(http_request)
	http_request.request_completed.connect(level_list_http_request_completed)
	# Perform a GET request. The URL below returns JSON as of writing.
	var error = http_request.request(str(url)+"level/all?proto_resp=f")
	if error != OK:
		push_error("An error occurred in the HTTP request.")
	# Perform a POST request. The URL below returns JSON as of writing.
	# Note: Don't make simultaneous requests using a single HTTPRequest node.
	# The snippet below is provided for reference only.

signal levels_fetched(list)

# Called when the HTTP request is completed.
func level_list_http_request_completed(result, response_code, headers, body):
	print(result)
	var json_object = JSON.new()
	json_object.parse(body.get_string_from_utf8())
	level_list = json_object.data["levels"]
	# Will print the user agent string used by the HTTPRequest node (as recognized by httpbin.org).
	levels_fetched.emit(level_list)




# only for windows now
func compress_level(name):
	var path = ProjectSettings.globalize_path("user://level/"+str(name))
	var result = ProjectSettings.globalize_path("user://level/"+str(name)+".zip")
	await OS.execute("powershell.exe",["Compress-Archive",path,result])
	
# only for windows now
func decompress_level(name):
	var path = ProjectSettings.globalize_path("user://level/"+str(name)+".zip")
	var result = ProjectSettings.globalize_path("user://level/")
	print("Decompressing "+str(path))
	await OS.execute("powershell.exe",["Expand-Archive",path,result,"-Force"])


	

func level_zip_file_path(name):
	var path = ProjectSettings.globalize_path("user://level/"+str(name)+".zip")
	return path

func upload_level(name):
	await compress_level(name)
	var http_request = HTTPRequest.new()
	add_child(http_request)
	
	http_request.request_completed.connect(upload_http_request_completed)
	
	
	var file = FileAccess.open(level_zip_file_path(name), FileAccess.READ)
	var file_content = file.get_buffer(file.get_length())
	
	var tn_file_name = "icon.png"
	var tnfile = FileAccess.open('res://%s' % tn_file_name, FileAccess.READ)
	var thumbnail_content = tnfile.get_buffer(tnfile.get_length())

	var body = PackedByteArray()
	body.append_array("\r\n--BodyBoundaryHere\r\n".to_utf8_buffer())
	body.append_array(("Content-Disposition: form-data; name=\"levelFiles\"; filename=\"%s\"\r\n" % name).to_utf8_buffer())
	body.append_array("Content-Type: application/zip\r\n\r\n".to_utf8_buffer())
	body.append_array(file_content)
	
	body.append_array("\r\n--BodyBoundaryHere\r\n".to_utf8_buffer())
	body.append_array(("Content-Disposition: form-data; name=\"thumbnail\"; filename=\"%s\"\r\n" % tn_file_name).to_utf8_buffer())
	body.append_array("Content-Type: image/png\r\n\r\n".to_utf8_buffer())
	body.append_array(thumbnail_content)
	
	body.append_array("\r\n--BodyBoundaryHere--\r\n".to_utf8_buffer())

	var headers =  [
		"Authorization: Bearer "+str(auth_token),
	"Content-Length: " + str(body.size()),
	"Content-Type: multipart/form-data; boundary=\"BodyBoundaryHere\""
	]

	
	var description = "This is a level".uri_encode()

	# Perform the HTTP request. The URL below returns a PNG image as of writing.
	var error = http_request.request_raw(str(url)+"level/?proto_resp=false&name="+str(name)+"&description="+str(description)+"&r=t&g=t&b=t&y=t", headers, true, HTTPClient.METHOD_POST, body)
	if error != OK:
		push_error("An error occurred in the HTTP request.")




func upload_http_request_completed(result, response_code, headers, body):
	var json_object: JSON = JSON.new()
	json_object.parse(body.get_string_from_utf8())
	print(json_object.data)

func login(username,password):
	# Create an HTTP request node and connect its completion signal.
	var http_request = HTTPRequest.new()
	add_child(http_request)
	
	http_request.request_completed.connect(login_http_request_completed)
	var body = "grant_type=&username="+str(username)+"&password="+str(password)+"&scope=&client_id=&client_secret="
	# Perform the HTTP request. The URL below returns a PNG image as of writing.
	var error = http_request.request(str(url)+"auth/token", ["Content-Type: application/x-www-form-urlencoded"], true, HTTPClient.METHOD_POST, body)
	if error != OK:
		push_error("An error occurred in the HTTP request.")


func get_auth_header():
	return ["Authorization: Bearer "+str(auth_token)]

func login_http_request_completed(result, response_code, headers, body):
	var json_object: JSON = JSON.new()
	json_object.parse(body.get_string_from_utf8())
	auth_token = json_object.data["access_token"]
	print("Login Success: "+str(auth_token))
	


func download_thumbnail(ulid):
	# Create an HTTP request node and connect its completion signal.
	var http_request = HTTPRequest.new()
	add_child(http_request)
	http_request.request_completed.connect(thumbnail_http_request_completed)
	# Perform the HTTP request. The URL below returns a PNG image as of writing.
	var error = http_request.request(str(url)+"level/pic?proto_resp=false&ulid="+str(ulid))
	if error != OK:
		push_error("An error occurred in the HTTP request.")


func fetch_api_data():
	# Create an HTTP request node and connect its completion signal.
	var http_request = HTTPRequest.new()
	add_child(http_request)
	http_request.request_completed.connect(data_http_request_completed)
	# Perform a GET request. The URL below returns JSON as of writing.
	var error = http_request.request(str(url)+"")
	if error != OK:
		push_error("An error occurred in the HTTP request.")
	# Perform a POST request. The URL below returns JSON as of writing.
	# Note: Don't make simultaneous requests using a single HTTPRequest node.
	# The snippet below is provided for reference only.


# Called when the HTTP request is completed.
func data_http_request_completed(result, response_code, headers, body):
	print(result)
	var json_object = JSON.new()
	json_object.parse(body.get_string_from_utf8())
	# Will print the user agent string used by the HTTPRequest node (as recognized by httpbin.org).
	print(json_object.data["message"])



# Called when the HTTP request is completed.
func thumbnail_http_request_completed(result, response_code, headers, body):
	var image = Image.new()
	var error = image.load_png_from_buffer(body)
	if error != OK:
		push_error("Couldn't load the image.")
	var texture = ImageTexture.new()
	texture.create_from_image(image)
	# Display the image in a TextureRect node.
	var texture_rect = TextureRect.new()
	add_child(texture_rect)
	texture_rect.texture = texture
