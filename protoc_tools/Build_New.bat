echo proto 文件必须是ansi格式， 这个工具不能识别utf8格式的。
echo 使用protobuffer 生成网络传输的结构体。
echo 生成java文件
protoc.exe --java_out=./ PbInfo.proto
echo 生成cs文件
ProtoGen.exe PbInfo.proto

echo 修改了此protobuff代码后，需要同步更新到java里的代码
pause