#### �������

����������� ���������� ���������� 50 ���������� �������� � 
����� https://rabota.yandex.ru/search?job_industry=275 (��� ������ 
������� ������������ �������). ���������� ������ ��������� �������� � 
���� ������ (�����, �� ����� ����������: MS Access, MS SQL Server, 
Postgres). ���������� ������ ������������ ������ ��� � �������������� � 
����� ������, ��� � ������ � ������������ � �� ����������.

��� ���������� ������������� ������������ .Net Core. ������������ EF � ������ CodeFirst. 
������������� ����� �������� 
�������, ����������� � Docker ����������. ������ ���� ������� readme � 
��������� ����������� � ���������� �� ������� ���������. 

### ����������� front

* front ������� � ��������� ����� (src-front)
* �������� SPA �������� - Vue/Vuex/Vue-CLI
* Dev-������ � ������ ������� ������ ����� "npm i || npm run serve"
* ����� ������ (Vue-CLI aka Webpack) ������������ ����� ����� ����������� ������

### ����������� back

* ���������� ��������� � ���� ������ web-������� - �������
* �������� ��������� - Asp .Net Core
* ����� ������� (������ ������������) - REST API
* �� - ������ Code First, ������������ ���������� � ����������� �� ������������ ������� (���� � ������, ���� �� MSSQL-connectionString)
* ������-������ ����������� ����� Command-pattern - IGetCommand, IExecuteCommand (������� ������ �� ������������ - �� ��� ����� ������������)
* �������� ������ - ������� �������� ������������ (aka Adapter-pattern) - IWebSourceLoader, ISourceParser (����� �� ������� �������� - ������������ �� �������)
* �������������� ������������ - � DiExtensions.cs
* �������� ������ ��� ������,�������,���������� - Web.Host.Service

### ������� ������ ����������

1. ��������� ������ Web.Host.Service (localhost:5000)
2. ��������� dev-������ ������(� �������� src-front) - "npm i || npm run serve" (localhost:8080)

### ������ �������������� �����������

* ��������� ������ **scripts\win\build-and-run-docker-infrastructure.bat**

�������� ����� ������ � �������

1. ������ Docker Image ��� ������� Web.Host.Service
2. ������ Docker Image ��� ������ ������� Nginx � ����������� ������ (front)
    * ������� "/" - index.html (�������� ����� ������� ��� �������� ������);
    * ������� "/api" - ������ �� ����:���� ������� Web.Host.Service
3. �������� ����������� (��������� ������� ������� - �����, ����������, ����) ������� - 
� ����� docker-compose.yml
4. ������ �������������� ���������� - docker-compose up
5. ������ chrome �� ����� http://$(doker-machine ip)/

�������� ��������:

* *build-and-run-docker-infrastructure.bat* <br>
������ ���� ����������� ������ ��� ������� docker-����������� � ������ �����������
* *build-di-proxy-front.bat*  <br>
������ Vue-App � ������� � docker-����� �� ������ ������ nginx <br>
 `scripts\win\app-front\` - ��������� ����� <br>
 `scripts\win\images\client-host-image.tar` - ����������� docker-����� (��. docker load)
* *build-di-webhost-service.bat* <br>
 ���������� ������� Web.Host.Service � �������  `scripts\win\app-webhost\` � ������� � docker-����� �� ������ ������ mcr.microsoft.com/dotnet/core/aspnet:2.2 <br>
 `scripts\win\images\web-host-image.tar` - ����������� docker-����� (��. docker load)
* *build-images.bat* <br>
������ ���� ����������� docker-�������
* *run-docker-compose.bat* <br>
������ �������������� ����������� <br>
`docker-compose.yml` - �������� ����������� �����������
`app-proxy-front-compose.Dockerfile` ������� ��� ������� ������, ��� ������������� ��������� �������<br>
`app-webhost-compose.Dockerfile` ������� ��� ������� webhost, ��� ������������� ��������� ������� <br>
`nginx.conf` ������������ ���-������� � ������������ ������-��������������


���������:

Docker ��� ��������� �� Win7 ���������� Docker Toolbox for Windows. 
� ��������� ���� ��������� ����������� docker-������ (VirtualBox boot2docker), �� ������� 
����������� ��� docker-������� � ����������� ����������. 

� ���� ����, ��� ������������� ������ ������� �� ��������� do�ker, 
�������� ������ � ���������� �������. 

�������� 
* ��� ������� � ����������� ����� ���������� ������� ������������ ����� ����� (����� �������) � �� ������������ � boot2docker
```
sudo mkdir --parents /c
sudo mount -t vboxsf C_DRIVE /c
# C_DRIVE - shared folder "C:\" in VirtualBox settings
```

* �������� ���� � ��������� ������ ���������� (� ��������� ��������� ������� ����������) <br>
������� ���������� ����������