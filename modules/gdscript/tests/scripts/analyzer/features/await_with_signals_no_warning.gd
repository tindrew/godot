# https://github.com/redotengine/redot/issues/54589
# https://github.com/redotengine/redot/issues/56265

extends Resource

func test():
	print("okay")
	await self.changed
	await unknown(self)

func unknown(arg):
	await arg.changed
