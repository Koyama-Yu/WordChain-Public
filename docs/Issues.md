# To Fix

プロパティの書き方が統一されていないと思うので後でちゃんと見直す

Player の State類のExecuteは大体共通のものが多いのでうまいこと継承とかインターフェース使って楽できない?

State管理において, 少し甘いところあり

JumpのStateの優先順位が高い?

jumpしててもtired, 逆もしかり

tiredはこのstate管理から外すべきかも

移動関係はWalkとかDashとかではなく一括でMoveとかにする

-> subStateとしてWalk, Dash, TiredMoveとかで実装の方が良い?

~そもそもの座標計算(targetまでの経路計算?)がおかしいっぽい~

-> targetまでではなく, targetまでの差分ベクトルをsetdistinationしていた(なぜ?)

targetのpositionを与えるようにしたらついてくるようになった

(差分ベクトルにしたら目を合わせなくなったのでこれはこれでどこかで使えそうではある)


Idle時でもPathを設定してしまっているため, 動いてしまう

StateにSetDistinationも含めてしまう? -> chaseにはtargetも渡す必要があり面倒くさい(煩雑になりそう)

-> コンストラクタを複数設定(引数異なる)にすればまだきれい?

でもこうなるとmovementの意義は?
