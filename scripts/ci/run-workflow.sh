#!/usr/bin/env bash
# Runs one workflow under act and fails if act silently skipped a job because
# .actrc has no image for its runs-on label.
set -euo pipefail

project_root="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"

echo "Running under act"

status=0

workflow="${1:?usage: run-workflow.sh <workflow> [act args...]}"
shift

output="$(act -W "${project_root}/.github/workflows/${workflow}.yml" "$@" 2>&1 | tee /dev/tty)" || status=$?

if grep -q "Skipping unsupported platform" <<<"$output"; then
    echo "ERROR: A job was skipped - .actrc is out of sync with runs-on in ${workflow}.yml" >&2
    exit 1
fi

exit "${status}"
