FROM node:22-alpine AS base
WORKDIR /WebClient
COPY package*.json ./
RUN npm install

FROM base AS build
COPY . .
RUN npm run build

FROM node:22-alpine AS production
WORKDIR /WebClient
COPY --from=build /WebClient ./
RUN npm install --production
CMD ["npm", "start"]

FROM base AS development
COPY . .
CMD ["npm", "run", "dev"]
