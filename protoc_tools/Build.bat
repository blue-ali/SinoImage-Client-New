echo proto �ļ�������ansi��ʽ�� ������߲���ʶ��utf8��ʽ�ġ�
echo ʹ��protobuffer �������紫��Ľṹ�塣
echo ����java�ļ�
protoc.exe --java_out=./ Logos.FileTransfer.proto 
echo ����cs�ļ�
ProtoGen.exe Logos.FileTransfer.proto

echo �޸��˴�protobuff�������Ҫͬ�����µ�java��Ĵ���
pause