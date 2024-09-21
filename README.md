# ExportMemoFromDocomoMemo
ドコモのメモ・スケジュールからメモをテキスト形式で出力するツール

## 概要
NTT Docomoが提供している「スケジュール＆メモ」アプリからエクスポートしたVCSファイルから、
メモ情報だけを抽出し、エクスポートしてテキストファイルに出力します。

※スケジュール＆メモアプリに重要な情報を記載している場合、
Webサイトでのコンバーターの利用は不安な点があったため、制作しました。

## 使い方
※WindowsOS上で使うことを前提としています。

１．以下から、実行ファイルをダウンロードします。
[download](https://github.com/kazu-u/ExportMemoFromDocomoMemo/releases/download/1.0.0/ExportMemoFromDocomoMemo.zip)

２．回答したディレクトリにアクセスし、右クリック→「ターミナルで開く」を実行します。

３．以下のコマンドを実行します。
.\ExportMemoFromDocomoMemo.exe -i vcsファイルのパス

例）.\ExportMemoFromDocomoMemo.exe -i "C:\Users\ユーザ名\Downloads\20240912194721.vcs"

４．正常に終了した場合、vcsファイルと同じディレクトリ内に、exportMemo.txt というファイルが出力されます。

## 注意事項
本ツールを作成した背景として、機種変更した際、新しい携帯ではdocomoのスケジュール＆メモアプリが利用できなく、
メモに重要な情報を残していたため、やむなくPC上でエクスポートできるようにしました。
そのため、私がエクスポートしたファイルでのテストを行い、正常にエクスポートできることは確認しましたが、
それ以上の確認はできていません。
