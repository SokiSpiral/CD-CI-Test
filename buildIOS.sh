echo "ビルド開始..."
export UNITY_APP_PATH="/Applications/Unity/Hub/Editor/6000.0.37f1/Unity.app/Contents/MacOS/Unity"
export UNITY_PROJECT_PATH="../../../../URP"
export UNITY_BUILDFUNC_NAME="BuildScript.BuildiOS"
export BUILD_OUTPUT_PATH="$UNITY_PROJECT_PATH/Build/"
export BUILD_DATE=$(date +"%Y%m%d_%H%M%S")

export IOS_BUILD_PATH="$BUILD_OUTPUT_PATH/iOSBuild"
export IPA_OUTPUT_PATH="$IOS_BUILD_PATH/Output"
export XCODE_PROJECT_PATH="$IOS_BUILD_PATH/Unity-iPhone.xcodeproj"
export XCODE_ARCHIVE_PATH="$IOS_BUILD_PATH/Unity-iPhone.xcarchive"
export EXPORT_PLIST_PATH="$BUILD_OUTPUT_PATH/export.plist"

export FIREBASE_APP_DISTRIBUTION_APP_ID="1:196709384800:ios:fda83f4c5160040d993905"

echo "Unityビルド実行中..."
$UNITY_APP_PATH -batchmode \
  -quit \
  -projectPath $UNITY_PROJECT_PATH \
  -executeMethod $UNITY_BUILDFUNC_NAME \
  -logFile ./buildIOS.log
  
if [ $? -eq 0 ]; then
  echo "ビルド成功。"
else
  echo "ビルド失敗。ログを確認してください。"
  exit 1
fi

echo "xcodeビルドを開始"

xcodebuild clean -project $XCODE_PROJECT_PATH \
-scheme Unity-iPhone -configuration Release -sdk iphoneos

xcodebuild archive -project $XCODE_PROJECT_PATH \
-scheme Unity-iPhone -configuration Release \
-archivePath $XCODE_ARCHIVE_PATH -sdk iphoneos

xcodebuild -exportArchive \
  -archivePath $XCODE_ARCHIVE_PATH \
  -exportPath $IPA_OUTPUT_PATH \
  -exportOptionsPlist $EXPORT_PLIST_PATH
  
if [ $? -eq 0 ]; then
  echo "ビルド成功。"
else
  echo "ビルド失敗。"
  exit 1
fi

echo "名前変更"
mv $IPA_OUTPUT_PATH"/URP.ipa" $BUILD_OUTPUT_PATH"/build_${BUILD_DATE}.ipa"

echo "Firebase App Distributionへアップロード"
firebase appdistribution:distribute $BUILD_OUTPUT_PATH"/build_${BUILD_DATE}.ipa" --app $FIREBASE_APP_DISTRIBUTION_APP_ID --groups tester
echo "アップロード完了！"
