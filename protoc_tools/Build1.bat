echo proto �ļ�������ansi��ʽ�� ������߲���ʶ��utf8��ʽ�ġ�
echo ʹ��protobuffer �������紫��Ľṹ�塣
echo ����java�ļ�
protoc.exe --java_out=./ PbInfo.proto
echo ����cs�ļ�
ProtoGen.exe PbInfo.proto

echo �޸��˴�protobuff�������Ҫͬ�����µ�java��Ĵ���
pause