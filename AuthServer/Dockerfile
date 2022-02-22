#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AuthServer/AuthServer.csproj", "AuthServer/"]
COPY ["DatabaseAccessor/DatabaseAccessor.csproj", "DatabaseAccessor/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["AspNetCoreSharedComponent/AspNetCoreSharedComponent.csproj", "AspNetCoreSharedComponent/"]
RUN dotnet restore "AuthServer/AuthServer.csproj"
COPY . .
WORKDIR "/src/AuthServer"
RUN dotnet build "AuthServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuthServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV CLIENT_AUTH_CONNECTION_STRING=${CLIENT_AUTH_CONNECTION_STRING}
ENV TEAM13_CONNECTION_STRING=${TEAM13_CONNECTION_STRING}
ENV GMAIL_USERNAME=${GMAIL_USERNAME}
ENV GMAIL_PASSWORD=${GMAIL_PASSWORD}
CMD ASPNETCORE_URLS=http://*:$PORT dotnet AuthServer.dll