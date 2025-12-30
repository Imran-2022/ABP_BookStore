# Stage 1 — Build stage

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
# Sets the working directory inside the container
COPY ["src/", "src/"]
# Copies your entire source folder into the container
COPY ["common.props", "./"]
COPY ["NuGet.Config", "./"]
RUN dotnet restore "src/Acme.BookStore.DbMigrator/Acme.BookStore.DbMigrator.csproj"
# Restores NuGet packages for DbMigrator project only
# Downloads dependencies into NuGet cache
COPY . .
# Copies everything else into the container
RUN dotnet publish "src/Acme.BookStore.DbMigrator/Acme.BookStore.DbMigrator.csproj" -c Release -o /app/publish

# Builds and publishes the app
# -c Release → optimized build
# -o /app/publish → output directory


# Stage 2 — Runtime stage

FROM mcr.microsoft.com/dotnet/aspnet:10.0
# Contains only what’s needed to run the app
WORKDIR /app
COPY --from=build /app/publish .

# Copies published output from build stage
# This is why it’s called a multi-stage build
# Final image contains:
# Only runtime files
# No source code
# No SDK

ENV ASPNETCORE_URLS=http://+:80
# Configures ASP.NET Core to:
# Listen on port 80
ENTRYPOINT ["dotnet", "Acme.BookStore.DbMigrator.dll"]

# Defines the command that runs when the container starts
# Starts the DbMigrator application
# This container will:
# Start
# Run database migrations
# Exit (usually)