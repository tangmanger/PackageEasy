# 基于NSIS打包可视化工具
##  开发的初衷
* 原有HM NIS Edit 创建安装程序很简单，但是不便于修改，新增。
* 原有nsi脚本直接新增组件，或者删除组件 删除文件等操作不简洁
* 最重要一点  想做点什么玩玩~ 哈哈
## 软件架构
  采用WPF+MVVM 来进行开发，仿Visual Studio 风格（虽然仿的不是那么彻底 -_-!）,基于.net 6开发，支持多工程同时编辑，绑定文件格式为 __.pge__ 文件格式。目前还有较多业务场景没有覆盖，还有些小bug，后期会慢慢维护完善。
## 版本说明
### V1.0
* 支持自定义工程创建安装
* 支持文件格式自定义
* 支持进程检测，进行安装提示
* 支持组件可视化编辑
* 支持打包插件安装
* 支持最近打开文件快捷访问
* 支持编译前校验
* 还有好多~ 
## 使用方法
晚些时候出个教程 感谢观看~
