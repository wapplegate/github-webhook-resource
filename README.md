# GitHub Webhook Resource

This Concourse resource creates GitHub webhooks for your GitHub repositories and supports custom webhook base urls and webhook secrets.

[![dockeri.co](https://dockerico.blankenship.io/image/wapplegate/github-webhook-resource)](https://hub.docker.com/r/wapplegate/github-webhook-resource)

https://hub.docker.com/r/wapplegate/github-webhook-resource/tags

## Usage

### Configure Resource, Resource Type, and Job

```yml
resource_types:
  - name: github-webhook-resource
    type: registry-image
    source:
      repository: wapplegate/github-webhook-resource
      tag: latest

resources:
  - name: github-webhook
    type: github-webhook-resource
    icon: github
    source:
      github_base_url: https://api.github.com
      github_token: ((personal_access_token))
  
jobs:
  - name: create-github-webhooks
    plan:
      - put: create-pull-request-webhook
        resource: github-webhook
        params:
          owner: my_organization
          repository: my_repository
          resource_name: pull-request
          webhook_base_url: https://ci-server.com
          webhook_token: ((github_webhook_token))
          events: [push, pull_request]
          webhook_secret: ((webhook_secret))
```
