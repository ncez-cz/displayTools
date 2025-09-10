const path = require("path");

module.exports = [
    {
        extends: path.resolve(__dirname, "./webpackBase.config.cjs"),
        entry: "./HtmlResources/src/site.ts",
        output: {
            path: path.resolve(__dirname, "HtmlResources/dist"),
            publicPath: "/",
        }
    },
    {
        extends: path.resolve(__dirname, "./webpackBase.config.cjs"),
        entry: "./Widgets/RazorComponents/isolated-razor-css.ts",
        output: {
            path: path.resolve(__dirname, "Widgets/RazorComponents"),
            publicPath: "/",
        }
    }
];
