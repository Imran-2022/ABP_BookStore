FROM node:20-alpine AS build

WORKDIR /app
COPY package*.json /app/

RUN yarn
COPY . /app

RUN yarn build:prod

FROM nginx:alpine
COPY dynamic-env.json /usr/share/nginx/html
COPY nginx.conf  /etc/nginx/conf.d/default.conf

RUN rm -f /usr/share/nginx/html/index.html
COPY --from=build /app/dist/browser/ /usr/share/nginx/html/
