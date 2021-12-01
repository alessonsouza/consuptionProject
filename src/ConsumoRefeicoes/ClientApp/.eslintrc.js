module.exports = {
  env: {
    browser: true,
    es2021: true,
  },
  extends: [
    'airbnb',
    'prettier',
  ],
  parserOptions: {
    ecmaFeatures: {
      jsx: true,
    },
    ecmaVersion: 12,
    sourceType: 'module',
  },
  plugins: [
    'prettier',
  ],
  rules: {
    "no-console": 0,
    "max-len": ["error", { "code": 100 }],
    "prefer-promise-reject-errors": ["off"],
    "react/jsx-filename-extension": ["off"],
    "react/jsx-closing-bracket-location": [0, "tag-aligned"],
    "react/jsx-props-no-spreading": "off",
    "react/prop-types": ["off"],
    "no-return-assign": ["off"],
    "arrow-body-style": 0,
    "no-param-reassign": 0
  },
};
