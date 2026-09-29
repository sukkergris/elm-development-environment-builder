#!/usr/bin/env bash
# Fails unless the pushed tag is exactly v<IMAGE_TAG>, where IMAGE_TAG comes
# from build/.env. The git tag and the Docker image tag then always carry the
# same number.
set -euo pipefail

tag_prefix='v'
tag="${1:?usage: check-release-tag.sh <tag>}"
script_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
image_tag="$(grep -E '^IMAGE_TAG=' "$script_dir/../../build/.env" | cut -d= -f2)"
expected="${tag_prefix}${image_tag}"

if [[ "$tag" != "$expected" ]]; then
  echo "::error::Tag '$tag' does not match IMAGE_TAG '$expected' from build/.env." >&2
  exit 1
fi

echo "Tag '$tag' matches IMAGE_TAG."
