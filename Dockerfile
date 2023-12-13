FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS publish

RUN apk add clang build-base zlib-dev

COPY ["GitHubWebhookResource/Out/Out.csproj", "GitHubWebhookResource/Out/"]

RUN dotnet restore "GitHubWebhookResource/Out/Out.csproj" --runtime linux-musl-x64

COPY . .

RUN dotnet publish ./GitHubWebhookResource/Out/Out.csproj -c Release -p:DebugType=None --runtime linux-musl-x64 -o /opt/resource

FROM alpine AS final

RUN apk add --no-cache libstdc++ icu-libs

COPY --from=publish /opt /opt

WORKDIR /opt/resource
ADD binaries .

RUN chmod +x check in out