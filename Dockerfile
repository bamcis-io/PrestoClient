FROM mcr.microsoft.com/dotnet/sdk:3.1

WORKDIR /app
COPY PrestoClient/PrestoClient.csproj PrestoClient/PrestoClient.csproj
COPY PrestoClientTests/PrestoClient.Tests.csproj PrestoClientTests/PrestoClient.Tests.csproj
RUN dotnet restore PrestoClient && dotnet restore PrestoClientTests

COPY PrestoClient PrestoClient
COPY PrestoClientTests PrestoClientTests
