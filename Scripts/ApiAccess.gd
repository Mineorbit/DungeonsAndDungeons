extends Node


var url = "https://mstillger.de/api/"

# Called when the node enters the scene tree for the first time.
func _ready():
	fetch_api_data()

# only for windows now
func compress_level(name):
	var path = ProjectSettings.globalize_path("user://level/"+str(name))
	var result = ProjectSettings.globalize_path("user://level/"+str(name)+".zip")
	OS.execute("powershell.exe",["Compress-Archive",path,result])
	
# only for windows now
func decompress_level(name):
	var path = ProjectSettings.globalize_path("user://level/"+str(name)+".zip")
	var result = ProjectSettings.globalize_path("user://level/")
	OS.execute("powershell.exe",["Expand-Archive",path,result])
	


func download_level(ulid):
	pass

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
