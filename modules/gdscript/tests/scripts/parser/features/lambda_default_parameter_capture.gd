# https://github.com/Redotengine/Redot/issues/56751

func test():
	var x = "local"
	var lambda = func(param = x):
		print(param)
	lambda.call()
