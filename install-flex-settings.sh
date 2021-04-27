#!/bin/bash
# Usage: 
# sudo curl -sSL https://bin.moe/s/install-flex-settings.sh | bash /dev/stdin

RED='\033[0;31m'
NC='\033[0m'

echo "${RED}Updating package repository and installing deps...${NC}"
sudo apt-get update && sudo apt-get install vim wget -y

cd ~ && mkdir flexpool-agent && cd flexpool-agent

echo "${RED}Writing example config...${NC}"
wget -q --show-progress https://raw.githubusercontent.com/binn/flexpool-tools/v1.1.0-beta1/flex.example.conf -O "flex.conf"

echo "${RED}Installing flex-settings-agent...${NC}"
wget -q --show-progress https://github.com/binn/flexpool-tools/releases/download/v1.1.0-beta1/flex-settings-agent -O "flex-settings-agent"
sudo chmod 777 flex-settings-agent
sudo mv flex-settings-agent /bin/flex-settings-agent

echo "${RED}Installed!"
echo "Make sure to edit your flex.conf using 'vim flex.conf' and update your desired values in there"
echo "You can use [ flex-settings-agent conf --file="~/flexpool-agent/flex.conf" ] to run the agent${NC}"

flex-settings-agent conf --file="~/flexpool-agent/flex.conf"