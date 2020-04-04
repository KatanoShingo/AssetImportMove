[![MIT License](http://img.shields.io/badge/license-MIT-blue.svg?style=flat)](LICENSE)
[![BCH compliance](https://bettercodehub.com/edge/badge/KatanoShingo/AssetImportMove?branch=master)](https://bettercodehub.com/)  

*AssetImportMove*
====

アセットをインポート時に任意の指定したファイル下に移動するアセット

## 📖概要
アセットストアで買ったアセットは `ssets/AssetStore/`のような別フォルダに置きたいでも、毎回外部のアセットをインポートするたびに手動で移動させるのめんどくさい。
git管理している場合新しいアセットをgitignoreで除するのも手間なので作りました。

## 💃Demo
![アニメーション](https://user-images.githubusercontent.com/40855834/78264061-acc0e900-753d-11ea-9abf-783f3e47c5bf.gif)

## 💻要件
Unity2018.4.15f1にて作成

## 🏃使い方
- unitypackageをインポートします。
- メニューバーから`Window` > `Asset Import Move Setting`をクリック
- `Asset Import Move Window`が出てくるので、 アセットを移動させたいフォルダを`Project Window`からドラッグ＆ドロップで登録
- 登録した`Asset Import Move Window`のフォルダアイコンをマウス中を押下で登録解除
- `Asset Import Move Window`にフォルダが登録されている間は機能し、登録されていない場合は機能しません
- `Asset Import Move Window`のhelpボタン押下で操作方法を確認できます

## 🎁unitypackage
[AssetImportMove.unitypackage](https://github.com/KatanoShingo/AssetImportMove/releases)

## 💪貢献
- バグを見つけた場合は、Issuesを開いてください。    
- 機能のリクエストがある場合は、問題を開いてください。    
- 貢献したい場合は、プルリクエストを送ってください。    

## 🔓ライセンス

[MIT](https://github.com/KatanoShingo/AssetImportMove/blob/master/LICENSE)

## 🐦著者
[@shi_k_7](https://twitter.com/shi_k_7)
