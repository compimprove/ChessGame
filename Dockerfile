FROM node:14 as build
WORKDIR /usr/src/app
COPY . .
RUN rm package-lock.json
RUN npm install
RUN npm run build
RUN rm -r node_modules

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build-env
WORKDIR /App

COPY --from=build /usr/src/app ./
# Restore as distinct layers
# COPY . .
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:3.1
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "ChessGame.dll"]