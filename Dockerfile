FROM microsoft/dotnet:2.1-sdk AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.sln .
COPY TransferoTeam/*.csproj ./TransferoTeam/
COPY TransferoTeam/*.csproj ./TransferoTeam/
COPY TransferoTeam.Tests/*.csproj ./TransferoTeam.Tests/
RUN dotnet restore

# Copy everything else and build
COPY TransferoTeam/. ./TransferoTeam/
COPY TransferoTeam.Tests/. ./TransferoTeam.Tests/
WORKDIR /app/TransferoTeam
RUN dotnet publish -c Release -o publish

# Build runtime image
FROM microsoft/dotnet:2.1-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=build-env /app/TransferoTeam/publish ./
ENTRYPOINT ["dotnet", "TransferoTeam.dll"]
