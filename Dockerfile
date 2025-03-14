FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS publish

RUN apk add clang build-base zlib-dev

COPY ["GitHubWebhookResource/Out/Out.csproj", "GitHubWebhookResource/Out/"]

RUN dotnet restore "GitHubWebhookResource/Out/Out.csproj" --runtime linux-musl-x64

COPY . .

RUN dotnet publish ./GitHubWebhookResource/Out/Out.csproj -c Release -p:DebugType=None --runtime linux-musl-x64 -o /opt/resource

FROM alpine AS final

RUN apk add --no-cache libstdc++ icu-libs jq

COPY --from=publish /opt /opt

COPY binaries/check /opt/resource/check
COPY binaries/in /opt/resource/in

RUN chmod +x /opt/resource/check /opt/resource/in /opt/resource/out
