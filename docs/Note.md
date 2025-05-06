# 実装の際に思ったことを書き留める用

## Stateの処理

- 平行ステートマシンや階層ステートマシンを使うと、今のフラグを使っているところももう少しすっきりする
- game programming paternsを参考
- 移動関係はWalkとかDashとかではなく一括でMoveとかにする -> subStateとしてWalk, Dash, TiredMoveとかで実装の方が良い?

## 敵の処理

のちの実装によるが単語検索を現在はハッシュにしているが, 候補を出力できるようにするためにはIndexOfや正規表現を使う必要がある

## その他思いついた直し事項

- ResourcesLoaderはsingletonでいいかも
- PlayerやEnemyはUnitBaseみたいな名前の基本クラスを作って共通事項をまとめる(メガホンも) -> それを継承してつくる形の方が良い?
- Enemyはさらに敵の種類ごとにEnemyControllerを継承?
- Movementもクラス継承で使えるとよさげである
- PauseとResumeはより汎用的にStopとかにした方が良い?
- canMoveとかのフラグ名変えたい(なるべく!記号をif文に書かないような名前に)
- InputBaseにHandleInput入れるか()メンバとかもそっちに含めれない?
- ゲームプレイ中のUIを管理するクラスの実装
- ポーズ中などに, 他のUI表示をON/OFFする(体力表示や右上のtab:pauseなど)

## その他

- targetまでの差分ベクトルをsetdistinationすると, 頑なに目を合わせようとしなくなる(偶然の産物だが、どこかで使えそう)
- はなれている間に、またUnity Sentisを使って音声処理的なのを実装している記事があったはずなので参考に
- 一度, Unity Sentisをガチりたい
- Nullチェックをもっといれるべき
