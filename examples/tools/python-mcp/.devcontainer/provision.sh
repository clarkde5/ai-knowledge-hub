#!/bin/bash
set -euxo pipefail

curl -LsSf https://astral.sh/uv/install.sh | sh

export NVM_DIR="$HOME/.nvm"
cat /tmp/provision/install.sh | bash
. "$NVM_DIR/nvm.sh"

nvm install node
