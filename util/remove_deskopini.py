import glob
import os

old_ishidden = glob._ishidden
glob._ishidden = lambda x: False
files = glob.glob('**/desktop.ini', recursive=True)
for file in files:
    print(file)
    os.remove(file)
glob._ishidden = old_ishidden