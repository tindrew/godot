#!/bin/sh

if [ ! -e "version.py" ]; then
  echo "This script should be ran from the root folder of the Redot repository."
  exit 1
fi

while getopts "h?sv:g:" opt; do
  case "$opt" in
  h|\?)
    echo "Usage: $0 [OPTIONS...]"
    echo
    echo "  -s script friendly file name (Redot.tar.gz)"
    echo "  -v Redot version for file name (e.g. 4.0-stable)"
    echo "  -g git treeish to archive (e.g. master)"
    echo
    exit 1
    ;;
  s)
    script_friendly_name=1
    ;;
  v)
    Redot_version=$OPTARG
    ;;
  g)
    git_treeish=$OPTARG
    ;;
  esac
done

if [ ! -z "$git_treeish" ]; then
  HEAD=$(git rev-parse $git_treeish)
else
  HEAD=$(git rev-parse HEAD)
fi

if [ ! -z "$script_friendly_name" ]; then
  NAME=Redot
else
  if [ ! -z "$Redot_version" ]; then
    NAME=Redot-$Redot_version
  else
    NAME=Redot-$HEAD
  fi
fi

CURDIR=$(pwd)
TMPDIR=$(mktemp -d -t Redot-XXXXXX)

echo "Generating tarball for revision $HEAD with folder name '$NAME'."
echo
echo "The tarball will be written to the parent folder:"
echo "    $(dirname $CURDIR)/$NAME.tar.gz"

git archive $HEAD --prefix=$NAME/ -o $TMPDIR/$NAME.tar

# Adding custom .git/HEAD to tarball so that we can generate VERSION_HASH.
cd $TMPDIR
mkdir -p $NAME/.git
echo $HEAD > $NAME/.git/HEAD
tar -uf $NAME.tar $NAME

cd $CURDIR
gzip -c $TMPDIR/$NAME.tar > ../$NAME.tar.gz

rm -rf $TMPDIR
