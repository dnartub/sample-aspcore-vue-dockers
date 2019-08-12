rem This script configure virtual docker-machine running at VirtualBox
rem docker-machine an VirtualBox installed via "Docker Toolbox for Windows"
rem Check all steps by https://docs.docker.com/toolbox/toolbox_install_windows/
rem and run script after Docker QuickStart Terminal configured network
rem check in command promt next commands:
rem - "docker-machine ip"
rem - "docker docker run hello-world"

cd C:\Program Files\Oracle\VirtualBox

call VBoxManage controlvm default poweroff soft
TIMEOUT /T 5


call VBoxManage sharedfolder remove default --name "CDRIVE"
call VBoxManage sharedfolder add default --name "CDRIVE" --hostpath "C:\\" --automount

VBoxManage startvm default

docker-machine ssh default "rm /var/lib/boot2docker/bootlocal.sh -f"
docker-machine ssh default "echo '#!/bin/sh' >> /var/lib/boot2docker/bootlocal.sh"
docker-machine ssh default "echo 'sudo mkdir --parents /c' >> /var/lib/boot2docker/bootlocal.sh"
docker-machine ssh default "echo 'sudo mount -t vboxsf CDRIVE /c' >> /var/lib/boot2docker/bootlocal.sh"
docker-machine ssh default "chmod +x /var/lib/boot2docker/bootlocal.sh"

call VBoxManage controlvm default poweroff soft
TIMEOUT /T 5
VBoxManage startvm default

pause