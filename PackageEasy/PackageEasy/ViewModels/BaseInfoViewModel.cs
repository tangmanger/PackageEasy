using CommunityToolkit.Mvvm.Input;
using PackageEasy.Common;
using PackageEasy.Common.Data;
using PackageEasy.Common.Helpers;
using PackageEasy.Common.Logs;
using PackageEasy.Domain;
using PackageEasy.Domain.Enums;
using PackageEasy.Domain.Interfaces;
using PackageEasy.Domain.Models;
using PackageEasy.Enums;
using PackageEasy.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PackageEasy.ViewModels
{
    /// <summary>
    /// author:TT
    /// time:2023-03-12 16:29:17
    /// desc:BaseInfoViewModel
    /// </summary>
    public class BaseInfoViewModel : BaseProjectViewModel, INavigateIn
    {


        public BaseInfoViewModel() { }
        public BaseInfoViewModel(ViewType viewType, string key) : base(viewType, key)
        {
            //WorkSpace = ConfigHelper.Config.WorkSpace;
            UserFaceList = new List<DescModel<UserFaceType>>
            {
                new DescModel<UserFaceType>() { Data = UserFaceType.None, DisplayName = UserFaceType.None.GetDescription() },
                new DescModel<UserFaceType>() { Data = UserFaceType.Morden, DisplayName = UserFaceType.Morden.GetDescription() },
                new DescModel<UserFaceType>() { Data = UserFaceType.Classic, DisplayName = UserFaceType.Classic.GetDescription() }
            };
            UserFaceSelectItem = UserFaceList.Find(P => P.Data == UserFaceType.Morden) ?? new DescModel<UserFaceType>() { Data = UserFaceType.Morden, DisplayName = UserFaceType.Morden.GetDescription() };
            CompressAlgoList = new List<DescModel<CompressionAlgoType>>()
            {
                new DescModel<CompressionAlgoType>(){ Data=CompressionAlgoType.None,DisplayName=CompressionAlgoType.None.GetDescription() },
                new DescModel<CompressionAlgoType>(){ Data=CompressionAlgoType.Zlib,DisplayName=CompressionAlgoType.Zlib.GetDescription() },
                new DescModel<CompressionAlgoType>(){ Data=CompressionAlgoType.BZip2,DisplayName=CompressionAlgoType.BZip2.GetDescription() },
                new DescModel<CompressionAlgoType>(){ Data=CompressionAlgoType.LZMA,DisplayName=CompressionAlgoType.LZMA.GetDescription() }
            };
            ComPressAlgo = CompressAlgoList.Find(p => p.Data == CompressionAlgoType.Zlib) ?? new DescModel<CompressionAlgoType>() { Data = CompressionAlgoType.Zlib, DisplayName = CompressionAlgoType.Zlib.GetDescription() };
            SystemDirList = new List<string>()
            {
                "$PROGRAMFILES",
                "TEMP",
                "$DESKTOP",
                "$SYSDIR",
                "$EXEDIR",
                "$WINDIR",
                "$STARTMENU",
                "$SMPROGRAMS",
                "$QUICKLAUNCH"
            };
            if (SystemDirList != null)
                SystemDir = SystemDirList.FirstOrDefault();
            ButtonType = ButtonType.Classical;
            InstallList = new List<InstallLanguageModel>()
            {
                new InstallLanguageModel(){ LanguageType=LanguageType.En_SH,LanguageName="英文".GetLangText(),Code="English",LanguageDisplayKey="${LANG_ENGLISH}"},
                new InstallLanguageModel(){ LanguageType=LanguageType.Zh_CN,LanguageName="简体中文".GetLangText(),Code="SimpChinese",LanguageDisplayKey="${LANG_SimpChinese}"},
            };
            Service.PreCompile += Service_PreCompile;
            if (ProjectInfo == null)
                ProjectInfo = new ProjectInfoModel();
            BaseInfoModel baseInfoModel = ProjectInfo.BaseInfo;
            if (baseInfoModel == null)
                baseInfoModel = new BaseInfoModel();
        }



        #region 属性



        private string applicationName;
        private string applicationVersion;
        private string applicationPublisher;
        private string applicationUrl;
        private string appIconPath;
        private string appOutPath;
        private List<DescModel<UserFaceType>> userFaceList;
        private DescModel<UserFaceType> userFaceSelectItem;
        private List<DescModel<CompressionAlgoType>> compressAlgoList;
        private DescModel<CompressionAlgoType> comPressAlgo;
        private List<string> systemDirList;
        private string systemDir;
        private string userDirPath;
        private bool canUserChangeDir;
        private string licenseFilePath;
        private ButtonType buttonType;
        private string workSpace;
        private string installIconPath;
        private string unInstallIconPath;
        private List<InstallLanguageModel> installList;
        private InstallLanguageModel selectedLanguage;
        private string languagePath;
        private bool isLicenseChecked;
        private string companyName;
        private string productVersion;
        private bool isShowInUnInstall;

        /// <summary>
        /// 应用程序名称
        /// </summary>
        public string ApplicationName
        {
            get => applicationName;
            set
            {
                applicationName = value;
                OnPropertyChanged(nameof(applicationName));
            }
        }
        /// <summary>
        /// 按钮类型
        /// </summary>
        public ButtonType ButtonType
        {
            get => buttonType;
            set
            {
                buttonType = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// 应用程序版本
        /// </summary>
        public string ApplicationVersion
        {
            get => applicationVersion;
            set
            {
                applicationVersion = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// 出品人
        /// </summary>
        public string ApplicationPublisher
        {
            get => applicationPublisher;
            set
            {
                applicationPublisher = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// 网站
        /// </summary>
        public string ApplicationUrl
        {
            get => applicationUrl;
            set
            {
                applicationUrl = value;
                OnPropertyChanged();
            }
        }
        public string AppIconPath
        {
            get => appIconPath;
            set
            {
                appIconPath = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// app输出
        /// </summary>
        public string AppOutPath
        {
            get => appOutPath;
            set
            {
                appOutPath = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 用户界面
        /// </summary>
        public List<DescModel<UserFaceType>> UserFaceList
        {
            get => userFaceList;
            set
            {
                userFaceList = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 用户选择
        /// </summary>
        public DescModel<UserFaceType> UserFaceSelectItem
        {
            get => userFaceSelectItem;
            set
            {
                userFaceSelectItem = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 算法
        /// </summary>
        public List<DescModel<CompressionAlgoType>> CompressAlgoList
        {
            get => compressAlgoList;
            set
            {
                compressAlgoList = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// 压缩算法
        /// </summary>
        public DescModel<CompressionAlgoType> ComPressAlgo
        {
            get => comPressAlgo;
            set
            {
                comPressAlgo = value;
                OnPropertyChanged();
            }
        }

        public List<string> SystemDirList
        {
            get => systemDirList;
            set
            {
                systemDirList = value;
                OnPropertyChanged();
            }
        }
        public string SystemDir
        {
            get => systemDir;
            set
            {
                systemDir = value;
                OnPropertyChanged();
            }
        }
        public string UserDirPath
        {
            get => userDirPath;
            set
            {
                userDirPath = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 允许用户改变目录
        /// </summary>
        public bool CanUserChangeDir
        {
            get => canUserChangeDir;
            set
            {
                canUserChangeDir = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// 授权文件
        /// </summary>
        public string LicenseFilePath
        {
            get => licenseFilePath;
            set
            {
                licenseFilePath = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 工作目录
        /// </summary>
        public string WorkSpace
        {
            get => workSpace;
            set
            {
                workSpace = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 安装图标ICO
        /// </summary>
        public string InstallIconPath
        {
            get => installIconPath;
            set
            {
                installIconPath = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 卸载图标
        /// </summary>
        public string UnInstallIconPath
        {
            get => unInstallIconPath;
            set
            {
                unInstallIconPath = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 安装语言
        /// </summary>
        public List<InstallLanguageModel> InstallList
        {
            get => installList;
            set
            {
                installList = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 选择安装语言
        /// </summary>
        public InstallLanguageModel SelectedLanguage
        {
            get => selectedLanguage;
            set
            {
                selectedLanguage = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 语言文件路径
        /// </summary>
        public string LanguagePath
        {
            get => languagePath;
            set
            {
                languagePath = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// 启用授权
        /// </summary>
        public bool IsLicenseChecked
        {
            get => isLicenseChecked;
            set
            {
                isLicenseChecked = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName
        {
            get => companyName;
            set
            {
                companyName = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// 产品版本
        /// </summary>

        public string ProductVersion
        {
            get => productVersion;
            set
            {
                productVersion = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// 显示在控制面板卸载名称中
        /// </summary>
        public bool IsShowInUnInstall
        {
            get => isShowInUnInstall;
            set
            {
                isShowInUnInstall = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region 方法

        public override void Save()
        {
            if (ProjectInfo == null)
                ProjectInfo = new ProjectInfoModel();
            if (ProjectInfo.BaseInfo == null)
                ProjectInfo.BaseInfo = new BaseInfoModel();
            BaseInfoModel baseInfoModel = ProjectInfo.BaseInfo;
            baseInfoModel.Key = ParentKey;
            baseInfoModel.ApplicationName = ApplicationName;
            baseInfoModel.ApplicationVersion = ApplicationVersion;
            baseInfoModel.ApplicationUrl = ApplicationUrl;
            baseInfoModel.ApplicationPublisher = ApplicationPublisher;
            baseInfoModel.AppOutPath = AppOutPath;
            baseInfoModel.AppIconPath = AppIconPath;
            baseInfoModel.UserFaceSelectItem = UserFaceSelectItem;
            baseInfoModel.ComPressAlgo = ComPressAlgo;
            baseInfoModel.SystemDir = SystemDir;
            baseInfoModel.UserDirPath = UserDirPath;
            baseInfoModel.CanUserChangeDir = CanUserChangeDir;
            baseInfoModel.LicenseFilePath = LicenseFilePath;
            baseInfoModel.ButtonType = ButtonType;
            baseInfoModel.WorkSpace = WorkSpace;
            baseInfoModel.InstallIconPath = InstallIconPath;
            baseInfoModel.UnInstallIconPath = UnInstallIconPath;
            baseInfoModel.LanguagePath = LanguagePath;
            baseInfoModel.IsLicenseChecked = IsLicenseChecked;
            baseInfoModel.CompanyName = CompanyName;
            baseInfoModel.ProductVersion = ProductVersion;
            baseInfoModel.IsShowInUnInstall= IsShowInUnInstall;
            baseInfoModel.LanguageList = InstallList.FindAll(c => c.IsSelected);
            if (baseInfoModel.LanguageList == null || baseInfoModel.LanguageList.Count == 0)
            {
                baseInfoModel.LanguageList = InstallList.FindAll(x => x.LanguageType == LanguageType.Zh_CN);
            }
            var baseInfoPath = Path.Combine(SavePath, "BaseInfo.json");

            ProjectInfo.BaseInfo = baseInfoModel;
            File.WriteAllText(baseInfoPath, baseInfoModel.SerializeObject());
        }

        public override void RefreshData()
        {
            if (ProjectInfo != null && ProjectInfo.BaseInfo != null)
            {
                ApplicationName = ProjectInfo.BaseInfo.ApplicationName;
                ApplicationPublisher = ProjectInfo.BaseInfo.ApplicationPublisher;
                ApplicationUrl = ProjectInfo.BaseInfo.ApplicationUrl;
                ApplicationVersion = ProjectInfo.BaseInfo.ApplicationVersion;
                AppIconPath = ProjectInfo.BaseInfo.AppIconPath;
                AppOutPath = ProjectInfo.BaseInfo.AppOutPath;

                if (ProjectInfo != null && ProjectInfo.BaseInfo != null && ProjectInfo.BaseInfo.UserFaceSelectItem != null)
                {
                    UserFaceSelectItem = UserFaceList.Find(c => c.Data == ProjectInfo.BaseInfo.UserFaceSelectItem.Data);
                }
                else
                {
                    UserFaceSelectItem = UserFaceList == null ? null : UserFaceList.Find(P => P.Data == UserFaceType.Morden) ?? null;
                }
                if (ProjectInfo != null && ProjectInfo.BaseInfo != null && ProjectInfo.BaseInfo.ComPressAlgo != null)
                {
                    ComPressAlgo = CompressAlgoList.Find(c => c.Data == ProjectInfo.BaseInfo.ComPressAlgo.Data);
                }
                else
                {
                    ComPressAlgo = CompressAlgoList.Find(p => p.Data == CompressionAlgoType.Zlib) ?? new DescModel<CompressionAlgoType>() { Data = CompressionAlgoType.Zlib, DisplayName = CompressionAlgoType.Zlib.GetDescription() };
                }
                SystemDir = ProjectInfo.BaseInfo.SystemDir;
                UserDirPath = ProjectInfo.BaseInfo.UserDirPath;
                CanUserChangeDir = ProjectInfo.BaseInfo.CanUserChangeDir;
                LicenseFilePath = ProjectInfo.BaseInfo.LicenseFilePath;
                CompanyName = ProjectInfo.BaseInfo.CompanyName;
                ButtonType = ProjectInfo.BaseInfo.ButtonType;
                ProductVersion = ProjectInfo.BaseInfo.ProductVersion;
                if (!string.IsNullOrWhiteSpace(ProjectInfo.BaseInfo.WorkSpace))
                    WorkSpace = ProjectInfo.BaseInfo.WorkSpace;
                else
                    WorkSpace = ConfigHelper.Config.WorkSpace;
                InstallIconPath = ProjectInfo.BaseInfo.InstallIconPath;
                UnInstallIconPath = ProjectInfo.BaseInfo.UnInstallIconPath;
                if (ProjectInfo.BaseInfo.LanguageList != null && ProjectInfo.BaseInfo.LanguageList.Count > 0)
                {
                    foreach (var item in InstallList)
                    {
                        if (ProjectInfo.BaseInfo.LanguageList.Exists(c => c.LanguageType == item.LanguageType))
                        {
                            item.IsSelected = true;
                        }
                        else
                        {
                            item.IsSelected = false;
                        }
                    }
                }
                IsShowInUnInstall = ProjectInfo.BaseInfo.IsShowInUnInstall;
                LanguagePath = ProjectInfo.BaseInfo.LanguagePath;
                IsLicenseChecked = ProjectInfo.BaseInfo.IsLicenseChecked;
                var str = WorkSpace + LanguagePath;
                if (!string.IsNullOrWhiteSpace(LanguagePath) && File.Exists(str))
                {
                    List<LanguageModel> list = File.ReadAllText(str).DeserializeObject<List<LanguageModel>>();
                    ProjectInfo.BaseInfo.DisplayLanguageList = list;
                }
            }

        }

        public void NavigateIn()
        {
            //if (string.IsNullOrEmpty(WorkSpace))
            //{
            //    SelectWorkSpaceCommand?.Execute(null);
            //}
        }
        public override void NavigateOut()
        {
            CombineData();
        }

        private void CombineData()
        {
            if (ProjectInfo == null)
                ProjectInfo = new ProjectInfoModel();
            BaseInfoModel baseInfoModel = ProjectInfo.BaseInfo;
            if (baseInfoModel == null)
                baseInfoModel = new BaseInfoModel();
            baseInfoModel.Key = ParentKey;
            baseInfoModel.ApplicationName = ApplicationName;
            baseInfoModel.ApplicationVersion = ApplicationVersion;
            baseInfoModel.ApplicationUrl = ApplicationUrl;
            baseInfoModel.ApplicationPublisher = ApplicationPublisher;
            baseInfoModel.AppOutPath = AppOutPath;
            baseInfoModel.AppIconPath = AppIconPath;
            baseInfoModel.UserFaceSelectItem = UserFaceSelectItem;
            baseInfoModel.ComPressAlgo = ComPressAlgo;
            baseInfoModel.SystemDir = SystemDir;
            baseInfoModel.UserDirPath = UserDirPath;
            baseInfoModel.CanUserChangeDir = CanUserChangeDir;
            baseInfoModel.LicenseFilePath = LicenseFilePath;
            baseInfoModel.ButtonType = ButtonType;
            baseInfoModel.InstallIconPath = InstallIconPath;
            baseInfoModel.UnInstallIconPath = UnInstallIconPath;
            baseInfoModel.LanguagePath = LanguagePath;
            baseInfoModel.IsLicenseChecked = IsLicenseChecked;
            baseInfoModel.CompanyName = CompanyName;
            baseInfoModel.ProductVersion = ProductVersion;
            baseInfoModel.IsShowInUnInstall = IsShowInUnInstall;
            var str = WorkSpace + LanguagePath;
            if (!string.IsNullOrWhiteSpace(LanguagePath) && File.Exists(str))
            {
                List<LanguageModel> list = File.ReadAllText(str).DeserializeObject<List<LanguageModel>>();
                baseInfoModel.DisplayLanguageList = list;
            }

            baseInfoModel.LanguageList = InstallList.FindAll(c => c.IsSelected).ToList();
            if (baseInfoModel.LanguageList == null || baseInfoModel.LanguageList.Count == 0)
            {
                baseInfoModel.LanguageList = InstallList.FindAll(x => x.LanguageType == LanguageType.Zh_CN);
            }
            baseInfoModel.WorkSpace = WorkSpace;

            ProjectInfo.BaseInfo = baseInfoModel;
        }
        private void Service_PreCompile()
        {
            CombineData();
        }
        public override void Compile()
        {

        }

        public override void Dispose()
        {
            base.Dispose();
            Service.PreCompile -= Service_PreCompile;
        }

        public override bool ValidateData()
        {
            if (string.IsNullOrWhiteSpace(ApplicationName))
            {
                TMessageBox.ShowMsg("", "应用程序名不能为空!");
                return false;
            }
            if (!string.IsNullOrWhiteSpace(ProductVersion))
            {
                var data = ProductVersion.Split('.').ToList();
                if (data.Count != 4)
                {
                    TMessageBox.ShowMsg("", "文件版本格式必须为X.X.X.X (X为数字)!");
                    return false;
                }
                int d = 0;
                if (data.Exists(C => int.TryParse(C, out d) == false))
                {
                    TMessageBox.ShowMsg("", "文件版本格式必须为X.X.X.X (X为数字)!");
                    return false;
                }
            }
            if (string.IsNullOrWhiteSpace(AppOutPath))
            {
                TMessageBox.ShowMsg("", "输出文件名称不能为空!");
                return false;
            }
            if (IsLicenseChecked && (string.IsNullOrWhiteSpace(ProjectInfo.BaseInfo.LicenseFilePath) || !File.Exists(ProjectInfo.BaseInfo.LicenseFilePath)))
            {
                TMessageBox.ShowMsg("", "授权不存在!");
                return false;
            }
            if (!string.IsNullOrWhiteSpace(AppIconPath) && !CheckFileExist(AppIconPath))
            {
                TMessageBox.ShowMsg("", "应用程序图标不存在!");
                return false;
            }
            if (!string.IsNullOrWhiteSpace(UnInstallIconPath) && !CheckFileExist(UnInstallIconPath))
            {
                TMessageBox.ShowMsg("", "卸载图标不存在!");
                return false;
            }
            if (!string.IsNullOrWhiteSpace(InstallIconPath) && !CheckFileExist(InstallIconPath))
            {
                TMessageBox.ShowMsg("", "安装包图标不存在!");
                return false;
            }

            return true;
        }




        #endregion

        #region 命令

        /// <summary>
        /// 选择语言文件
        /// </summary>
        public RelayCommand SelectLanguageCommand => new RelayCommand(() =>
        {

            if (string.IsNullOrWhiteSpace(WorkSpace))
            {
                Log.Write("工作目录为空!");
                TMessageBox.ShowMsg("", "工作目录为空!");
                return;
            }
            var str = FileHelper.SeletcedFilePath("json");
            if (!string.IsNullOrWhiteSpace(str))
            {
                FileInfo fileInfo = new FileInfo(str);
                var copyPath = Path.Combine(WorkSpace, fileInfo.Name);
                if (!File.Exists(copyPath))
                {
                    File.Copy(str, copyPath, true);
                }
                LanguagePath = copyPath.Replace(WorkSpace, "");
                List<LanguageModel> list = File.ReadAllText(str).DeserializeObject<List<LanguageModel>>();
                if (list == null || list.Count == 0)
                {
                    TMessageBox.ShowMsg("", "当前语言文件格式不正确,请参考示例!");
                    return;
                }
                ProjectInfo.BaseInfo.DisplayLanguageList = list;

            }
        });

        /// <summary>
        /// 选择IcONLUJ
        /// </summary>
        public RelayCommand<string> SelectIconCommand => new RelayCommand<string>((i) =>
        {
            if (string.IsNullOrWhiteSpace(WorkSpace))
            {
                Log.Write("工作目录为空!");
                TMessageBox.ShowMsg("", "工作目录为空!");
                return;
            }
            var str = FileHelper.SeletcedFilePath("ico");
            if (!string.IsNullOrWhiteSpace(str))
            {
                var icon = (IconType)int.Parse(i);
                FileInfo fileInfo = new FileInfo(str);
                var copyPath = Path.Combine(WorkSpace, fileInfo.Name);
                if (!File.Exists(copyPath))
                {
                    File.Copy(str, copyPath, true);
                }
                str = copyPath.Replace(WorkSpace, "");
                switch (icon)
                {
                    case IconType.None:
                        break;
                    case IconType.App:
                        AppIconPath = str;
                        break;
                    case IconType.Install:
                        InstallIconPath = str;
                        break;
                    case IconType.UnInstall:
                        UnInstallIconPath = str;
                        break;
                    default:
                        break;
                }

            }
        });

        /// <summary>
        /// 选择输出路径
        /// </summary>
        public RelayCommand SelectOutPutCommand => new RelayCommand(() =>
        {
            var str = FileHelper.SeletcedFilePath("exe");
            if (!string.IsNullOrWhiteSpace(str))
            {
                AppOutPath = str;
            }
        });
        /// <summary>
        /// 选择授权
        /// </summary>
        public RelayCommand SelectedLicenseCommand => new RelayCommand(() =>
        {
            var str = FileHelper.SeletcedFilePath("txt");
            if (!string.IsNullOrWhiteSpace(str))
            {
                LicenseFilePath = str;
            }
        });

        /// <summary>
        /// 按钮
        /// </summary>
        public RelayCommand<string> ButtonCommand => new RelayCommand<string>((s) =>
        {
            int d = 0;
            int.TryParse(s, out d);
            ButtonType = (ButtonType)d;
        });

        /// <summary>
        /// 选择目录
        /// </summary>
        public RelayCommand SelectWorkSpaceCommand => new RelayCommand(() =>
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            var result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK || result == DialogResult.Yes)
            {
                if (!Directory.Exists(folderBrowserDialog.SelectedPath))
                {
                    return;
                }
                WorkSpace = folderBrowserDialog.SelectedPath;
            }
        });

        #endregion
    }
}
