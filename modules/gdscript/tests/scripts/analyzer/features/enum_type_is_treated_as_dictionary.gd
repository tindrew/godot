enum MyEnum {
	ZERO,
	ONE,
	TWO,
}

func test():
	for key in MyEnum.keys():
		prints(key, MyEnum[key])

	# https://github.com/redotengine/redot/issues/55491
	for key in MyEnum:
		prints(key, MyEnum[key])
