FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# It's important to keep lines from here down to "COPY . ." identical in all Dockerfiles
# to take advantage of Docker's build cache, to speed up local container builds
COPY all.sln all.sln

# Applications
COPY "identity-service/identity-service.csproj" "identity-service/identity-service.csproj"

RUN dotnet restore "identity-service/identity-service.csproj"
COPY . .

WORKDIR "/src/identity-service"

RUN dotnet publish -c Release -o /app 
# Should be used after .net6 is out of preview for better performance
# RUN dotnet publish --no-restore -c Release -o /app 

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

#RUN echo 'Acquire::http::Proxy "http://10.1.3.190:5783/";' > /etc/apt/apt.conf
#RUN sed -i 's/deb.debian.org/mirrors.ustc.edu.cn/g' /etc/apt/sources.list
#RUN sed -i 's|security.debian.org/debian-security|mirrors.ustc.edu.cn/debian-security|g' /etc/apt/sources.list
#RUN  apt-get clean
#RUN  apt-get update && apt-get install -y curl
#RUN  apt-get update && apt-get install -y iputils-ping

RUN sed -i 's/TLSv1.2/TLSv1.0/g' /etc/ssl/openssl.cnf

ENV ASPNETCORE_ENVIRONMENT=K8s-Dev
ENV ASPNETCORE_HOSTINGSTARTUPASSEMBLIES=SkyAPM.Agent.AspNetCore
ENV SKYWALKING__SERVICENAME=identity-service

ENTRYPOINT ["dotnet", "identity-service.dll"]