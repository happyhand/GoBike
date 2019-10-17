const path = require("path");
const webpack = require("webpack");
const HtmlWebpackPlugin = require("html-webpack-plugin");
module.exports = {
  //這個webpack打包的對象，這裡面加上剛剛建立的index.js
  entry: "./src/main.jsx",
  output: {
    // path.join => 兩個參數，依當前的作業系統幫你在中間加 '/' 或 '\'，然後串接
    path: path.join(__dirname, "dist"),
    //這裡是打包後的檔案名稱
    filename: "bundle.js"
  },
  resolve: {
    extensions: [".js", ".jsx", "css", ".scss"]
  },
  //將loader的設定寫在module的rules屬性中
  module: {
    rules: [
      //第一個loader編譯JSX
      {
        test: /.jsx$/,
        exclude: /node_modules/,
        use: { loader: "babel-loader" }
      },
      //第二個loader編譯ES6
      {
        test: /.js$/,
        exclude: /node_modules/,
        use: { loader: "babel-loader" }
      },
      {
        test: /\.css$/,
        // '-loader' 可省略，'!' 表示 loader 串聯順序（由右往左依序轉換），'？' 表示傳送請求參數（類似 get）來進階設定
        // loader 串聯亦可用陣列表示：
        // loader: [ 'style-loader' , 'css-loader?sourceMap' ]
        loader: "style-loader!css-loader?sourceMap"
      },
      {
        test: /\.scss$/,
        loader: "style!css?sourceMap!sass?sourceMap"
      },
      {
        test: /\.less$/,
        loader: "style!css?sourceMap!less?sourceMap"
      },
      {
        test: /\.(jpe?g|JPE?G|png|PNG|gif|GIF|svg|SVG|woff|woff2|eot|ttf)(\?v=\d+\.\d+\.\d+)?$/,
        loader: "url?limit=1024&name=[sha512:hash:base64:7].[ext]"
      }
    ]
  },
  devtool: "cheap-module-eval-source-map",
  plugins: [
    new webpack.HotModuleReplacementPlugin(),
    new webpack.ProvidePlugin({
      $: "jquery",
      React: "react",
      ReactDOM: "react-dom"
    }),
    new HtmlWebpackPlugin({
      template: "./src/index.html"
    })
  ]
};
