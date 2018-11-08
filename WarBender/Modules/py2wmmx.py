#!/usr/bin/env python

# Copy this script into the directory with your module system, and run it like so:
#
#   py -2 py2wmmx.py module_constants.py >output.xml
#
# It will look at all variables defined in module_constants.py, and for each one
# of them, generate either a <slot> definition or an <option> definition for use
# in a .wmmx file. If the name of the variable starts with "slot_", then it will
# be a slot definition; otherwise, it's an option. 
#
# Since the script doesn't know which object or objects the slots belong to, or
# which <enum> or <flags> the options belong to, you will need to copy/paste the
# produced definitions into your .wmmx into the appropriate places.

from __future__ import print_function
from os import path
import sys
import numbers

if len(sys.argv) != 2:
    print('Usage:\n\tpy -2 py2wmmx.py <module.py>', file=sys.stderr)
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
