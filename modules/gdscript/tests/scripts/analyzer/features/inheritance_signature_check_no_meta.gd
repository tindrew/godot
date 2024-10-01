func test():
	print("ok")

# https://github.com/Redotengine/Redot/issues/71994
class A:
	extends RefCounted
class B:
	extends A
	func duplicate():
		pass
