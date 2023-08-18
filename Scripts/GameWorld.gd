extends Spatial

var focusin_callback = JavaScript.create_callback(self, "focusin")
var focusout_callback = JavaScript.create_callback(self, "focusout")

# Called when the node enters the scene tree for the first time.
func _ready():
	if OS.get_name() == "HTML5":
		JavaScript.get_interface("window").addEventListener("focusin", focusin_callback) 
		JavaScript.get_interface("window").addEventListener("focusout", focusout_callback)

func focusin(_event):
	get_tree().paused = false
	#print("focus_in")

func focusout(_event):
	get_tree().paused = true
	#print("focus_out")
