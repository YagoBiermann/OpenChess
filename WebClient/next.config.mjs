/** @type {import('next').NextConfig} */
const nextConfig = {
  webpack: (config, context) => {
    // Enable polling based on env variable being set
    if (process.env.NEXT_WEBPACK_USEPOLLING) {
      config.watchOptions = {
        aggregateTimeout: 200,
        poll: 1000,
      };
    }
    return config;
  },
};
export default nextConfig;
