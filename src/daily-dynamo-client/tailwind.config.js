/** @type {import('tailwindcss').Config} */

const defaultTheme = require("tailwindcss/defaultTheme");
module.exports = {
  darkMode: ["class"],
  content: ["./src/**/*.{html,ts}"],
  theme: {
    fontFamily: {
      sans: ["Poppins", ...defaultTheme.fontFamily.sans],
    },
    extend: {
      colors: {
        primary: "var(--clr-primary)",
        secondary: "var(--clr-secondary)",
        tertiary: "var(--clr-tertiary)",
        text: "var(--clr-text)",
        background: "var(--clr-background)",
        background_sec: "var(--clr-background-sec)",
      },
      boxShadow: {
        theme_form: "-10px -10px 30px #fff,10px 10px 30px #AEAEC0",
      },
      borderRadius: {
        theme: "var(--dd-border-radius)",
      },
    },
  },
  plugins: [],
  corePlugins: {},
};
