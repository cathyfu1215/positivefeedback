# Use the official .NET 8 SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory
WORKDIR /app

# Copy the .csproj file and restore dependencies
COPY ./*.csproj ./
RUN dotnet restore

# Copy all files and build the app
COPY . ./
RUN dotnet publish -c Release -o out

# Set the entry point for the app
ENTRYPOINT ["dotnet", "out/FeedbackApp.dll"]
