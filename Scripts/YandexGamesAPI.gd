extends Node


static func get_info():
	var javascript = JavaScript.get_interface("window")
	return javascript.info;

static func get_language():
	var javascript = JavaScript.get_interface("window")
	return javascript.ysdk.environment.i18n.lang;

static func show_ad():
	var javascript = JavaScript.get_interface("window")
	javascript.showAd();