import numpy,sys
exec('ans = ' + sys.stdin.read())
if isinstance(ans, numpy.matrix) or isinstance(ans, numpy.ndarray):
	print ans.tolist()
else:
	print ans