# production
FROM node:lts-alpine as prod

EXPOSE 4000
ENV NODE_ENV=production
WORKDIR /app

RUN apk add --no-cache --virtual .gyp python3 make g++ 

COPY package.json yarn.lock* ./
RUN yarn global add typescript 
RUN yarn global add prisma
RUN yarn install --production && yarn cache clean 
COPY . .

RUN prisma generate
RUN yarn migrate
RUN yarn global remove  prisma

RUN tsc --project ./tsconfig.json
# RUN yarn test

RUN apk del .gyp
RUN yarn global remove typescript 

RUN chown -R node /app
USER node

CMD ["node", "/app/out/index.js"]

#dev
FROM prod as dev
ENV NODE_ENV=development
RUN apk add --no-cache tini

RUN yarn install --production=false

ENTRYPOINT ["/sbin/tini", "--"]

CMD ["yarn", "develop"]

FROM dev as test
ENV NODE_ENV=development
RUN yarn test