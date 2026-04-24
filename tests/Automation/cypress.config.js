const { defineConfig } = require("cypress");

module.exports = defineConfig({
  allowCypressEnv: false,

  e2e: {
    viewportWidth: 1920,
    viewportHeight: 1080
  },
  
});
module.exports = defineConfig({
  e2e: {
    baseUrl: 'http://localhost:5173',
    env: {
      apiUrl: 'http://localhost:5000'
    }
  }
});
