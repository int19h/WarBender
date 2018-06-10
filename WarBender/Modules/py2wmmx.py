#!/usr/bin/env python

from __future__ import print_function
from os import path
import sys
import numbers

if len(sys.argv) != 2:
    print('Usage:\n\tpy -2 py2wmmx <module.py>', file=sys.stderr)
    sys.exit(1)

filename = sys.argv[1]
sys.path += [path.dirname(filename)]
source = open(filename).read()
code = compile(source, filename, 'exec')

class Locals(object):
    def __init__(self):
        self.writes = []

    def __getitem__(self, key):
        for k, v in self.writes:
            if k == key:
                return v
        return None

    def __setitem__(self, key, value):
        self.writes += [(key, value)]

locs = Locals()
eval(code, None, locs)
modvars = locs.writes

for name, value in modvars:
    if name.startswith('__'): continue
    if name.startswith('slot_'):
        print('<slot index="%s" name="%s" />' % (value, name[5:]))
    elif isinstance(value, numbers.Integral):
        print('<option name="%s" value="%s" />' % (name, value))
