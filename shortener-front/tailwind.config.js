import flowbite from "flowbite-react/tailwind";

/** @type {import('tailwindcss').Config} */
export default {
    content: [
        "./src/**/*.{js,jsx,ts,tsx}",
        "./index.html",
        "node_modules/flowbite-react/lib/esm/**/*.js",
        flowbite.content()
    ],
    theme: {
        extend: {},
    },
    plugins: [
        flowbite.plugin()
    ],
}

