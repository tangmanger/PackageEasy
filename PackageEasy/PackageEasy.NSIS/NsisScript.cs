using PackageEasy.Domain;
using PackageEasy.Domain.Enums;
using PackageEasy.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace PackageEasy.NSIS
{
    public class NSISScript : BaseBuildScript
    {
        public override string CompilerName => "makensis";

        public override List<string> Build(ProjectInfoModel projectInfoModel, ViewType viewType)
        {
            throw new NotImplementedException();
        }
        List<LanguageModel> Languages { get; set; }
        string? GetLanguage(string? languageText, LanguageType languageType)
        {
            if (string.IsNullOrWhiteSpace(languageText)) { return languageText; }
            if (Languages == null || languageText.Length == 0) return languageText;
            var chLanguage = Languages.Find(c => c.LanguageType == LanguageType.Zh_CN && c.LanguageText == languageText);
            if (chLanguage == null) return languageText;
            var taget = Languages.Find(c => c.LanguageType == languageType && c.Id == chLanguage.Id);
            if (taget == null) return languageText;
            return taget.LanguageText;
        }
        public override List<string> Build(ProjectInfoModel projectInfoModel)
        {
            Languages = projectInfoModel.BaseInfo.DisplayLanguageList;
            List<string> list = new List<string>();
            list.Add($";描述:{"由工具PackageEasy生成"}");
            list.Add($";作者:{"Tangmanger"}");
            list.Add($";时间:{DateTime.Now.ToString("G")}");
            list.Add($";网站:https://www.dicgo.com");
            list.Add($";PackageEasy版本:{Application.ResourceAssembly?.GetName()?.Version?.ToString()}");
            if (projectInfoModel != null)
            {
                if (projectInfoModel.BaseInfo != null)
                {
                    var baseInfo = projectInfoModel.BaseInfo;
                    //             !define PRODUCT_NAME "PPPics"
                    //!define PRODUCT_VERSION "7.0"
                    //!define PRODUCT_PUBLISHER "PPPics, Inc."
                    //!define PRODUCT_WEB_SITE "http://www.pppics.cn"
                    // !define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\AppMainExe.exe"
                    //!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
                    //!define PRODUCT_UNINST_ROOT_KEY "HKLM"
                    //!define PRODUCT_STARTMENU_REGVAL "NSIS:StartMenuDir"
                    list.Add($"!define PRODUCT_NAME \"{baseInfo.ApplicationName}\"");
                    list.Add($"!define PRODUCT_VERSION \"{baseInfo.ApplicationVersion}\"");

                    if (!string.IsNullOrWhiteSpace(projectInfoModel.BaseInfo.ProductVersion))
                        list.Add($"!define PRODUCT_VERSION_A \"{projectInfoModel.BaseInfo.ProductVersion}\"");
                    list.Add($"!define PRODUCT_PUBLISHER $(CompanyName)");
                    list.Add($"!define PRODUCT_WEB_SITE {baseInfo.ApplicationUrl}");
                    list.Add($"!define PRODUCT_UNINST_ROOT_KEY \"HKLM\"");

                    bool isMorden = false;
                    if (projectInfoModel.BaseInfo.ComPressAlgo != null && projectInfoModel.BaseInfo.ComPressAlgo.Data != CompressionAlgoType.None && projectInfoModel.BaseInfo.ComPressAlgo.Data != CompressionAlgoType.Zlib)
                    {
                        list.Add($"SetCompressor {projectInfoModel.BaseInfo.ComPressAlgo.DisplayName.ToLower()}");
                    }
                    if (projectInfoModel.BaseInfo.UserFaceSelectItem != null && projectInfoModel.BaseInfo.UserFaceSelectItem.Data == UserFaceType.Morden)
                    {
                        isMorden = true;
                    }
                    var appNames = projectInfoModel?.FinishInfo?.ApplicationName.Split('\\');

                    list.Add($"!define PRODUCT_DIR_REGKEY \"Software\\Microsoft\\Windows\\CurrentVersion\\App Paths\\{appNames.LastOrDefault()}\"");
                    list.Add($"!define PRODUCT_UNINST_KEY \"Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\${{PRODUCT_NAME}}\"");
                    if (projectInfoModel.AppIcon.IsCanChangeStartMenuName)
                        list.Add($"!define PRODUCT_STARTMENU_REGVAL \"NSIS:StartMenuDir\"");
                    list.Add("!include \"nsProcess.nsh\"");
                    list.Add("!include \"LogicLib.nsh\"");
                    list.Add(" !include \"FileFunc.nsh\"");
                    if (isMorden)
                    {
                        list.Add("!include \"MUI.nsh\"");
                        list.Add("!define MUI_ABORTWARNING");
                    }


                    string iconPath = "${NSISDIR}\\Contrib\\Graphics\\Icons\\modern-install.ico";
                    if (!string.IsNullOrWhiteSpace(baseInfo.InstallIconPath))
                        iconPath = baseInfo.WorkSpace + baseInfo.InstallIconPath;
                    list.Add($"!define MUI_ICON \"{iconPath}\"");
                    string unInstallKet = "${NSISDIR}\\Contrib\\Graphics\\Icons\\modern-uninstall.ico";
                    if (!string.IsNullOrWhiteSpace(baseInfo.UnInstallIconPath))
                        unInstallKet = baseInfo.WorkSpace + baseInfo.UnInstallIconPath;
                    list.Add($"!define MUI_UNICON \"{unInstallKet}\"");
                    list.Add($"!define MUI_LANGDLL_REGISTRY_ROOT \"${{PRODUCT_UNINST_ROOT_KEY}}\"");
                    list.Add($"!define MUI_LANGDLL_REGISTRY_KEY \"${{PRODUCT_UNINST_KEY}}\"");
                    list.Add("!define MUI_LANGDLL_REGISTRY_VALUENAME \"NSIS:Language\"");
                    if (isMorden)
                    {
                        list.Add("; 欢迎界面");
                        list.Add("!insertmacro MUI_PAGE_WELCOME");
                    }

                    list.Add(";授权界面");
                    if (projectInfoModel.BaseInfo.IsLicenseChecked && !string.IsNullOrWhiteSpace(projectInfoModel.BaseInfo.LicenseFilePath) && File.Exists(projectInfoModel.BaseInfo.LicenseFilePath))
                    {
                        if (projectInfoModel.BaseInfo.ButtonType == ButtonType.Choose)
                        {
                            if (!isMorden)
                                list.Add($"LicenseForceSelection checkbox");
                            else
                            {
                                list.Add($"!define MUI_LICENSEPAGE_CHECKBOX");
                            }
                        }
                        else if (projectInfoModel.BaseInfo.ButtonType == ButtonType.Circle)
                        {
                            if (isMorden)
                            {
                                list.Add("!define MUI_LICENSEPAGE_RADIOBUTTONS");
                            }
                            else
                            {
                                list.Add($"LicenseForceSelection radiobuttons");
                            }
                        }
                        if (isMorden)
                        {
                            list.Add($"!insertmacro MUI_PAGE_LICENSE \"{projectInfoModel.BaseInfo.LicenseFilePath}\"");
                        }
                    }

                    if (isMorden)
                    {

                        list.Add("; 组件界面");
                        if (projectInfoModel.AssemblyInfo != null)
                        {
                            if (projectInfoModel.AssemblyInfo.IsAllowChoose)
                            {
                                list.Add("!insertmacro MUI_PAGE_COMPONENTS");
                            }
                        }
                        list.Add("; 目录界面");
                        if (projectInfoModel.BaseInfo.CanUserChangeDir)
                            list.Add("!insertmacro MUI_PAGE_DIRECTORY");
                        if (projectInfoModel.AppIcon != null && projectInfoModel.AppIcon.IsCanChangeStartMenuName)
                        {
                            list.Add("; Start menu page");
                            list.Add("var ICONS_GROUP");
                            list.Add($"!define MUI_STARTMENUPAGE_NODISABLE");
                            list.Add($"!define MUI_STARTMENUPAGE_DEFAULTFOLDER \"{projectInfoModel?.AppIcon?.StartMenuName}\"");
                            list.Add($"!define MUI_STARTMENUPAGE_REGISTRY_ROOT \"${{PRODUCT_UNINST_ROOT_KEY}}\"");
                            list.Add($"!define MUI_STARTMENUPAGE_REGISTRY_KEY \"${{PRODUCT_UNINST_KEY}}\"");
                            list.Add($"!define MUI_STARTMENUPAGE_REGISTRY_VALUENAME \"${{PRODUCT_STARTMENU_REGVAL}}\"");
                            list.Add($"!insertmacro MUI_PAGE_STARTMENU Application $ICONS_GROUP");
                        }
                        list.Add("; 安装界面");
                        list.Add("!insertmacro MUI_PAGE_INSTFILES");
                        if (projectInfoModel != null & projectInfoModel.FinishInfo != null)
                        {
                            list.Add("; 安装完成界面");
                            if (projectInfoModel.FinishInfo.IsAutoRun)
                            {
                                list.Add($"!define MUI_FINISHPAGE_RUN \"{projectInfoModel.FinishInfo.ApplicationName}\"");
                                list.Add($"!define MUI_FINISHPAGE_RUN_PARAMETERS \"{projectInfoModel.FinishInfo.RunParam}\"");
                            }
                            if (projectInfoModel.FinishInfo.IsShowReadme && !string.IsNullOrWhiteSpace(projectInfoModel.FinishInfo.ReadMeFileName))
                            {
                                list.Add($"!define MUI_FINISHPAGE_SHOWREADME \"{projectInfoModel.FinishInfo.ReadMeFileName}\"");
                            }
                        }

                        //安装完成后运行
                        //list.Add(";!define MUI_FINISHPAGE_RUN "$INSTDIR\ArchimedAnalyzer.exe"");
                        list.Add("!insertmacro MUI_PAGE_FINISH");
                        list.Add("; Uninstaller pages");
                        list.Add("!insertmacro MUI_UNPAGE_INSTFILES");

                    }
                    list.Add("; Language files");
                    if (projectInfoModel.BaseInfo.LanguageList != null)
                    {
                        foreach (var item in projectInfoModel.BaseInfo.LanguageList)
                        {
                            if (item.IsSelected)
                                list.Add($"!insertmacro MUI_LANGUAGE \"{item.Code}\"");
                        }
                    }
                    else
                    {
                        list.Add("!insertmacro MUI_LANGUAGE \"SimpChinese\"");
                        //list.Add("!insertmacro MUI_LANGUAGE \"English\"");
                    }
                    list.Add(";多语言开始");
                    if (projectInfoModel.BaseInfo.LanguageList != null)
                    {
                        if (projectInfoModel.AssemblyInfo != null)
                        {
                            if (projectInfoModel.AssemblyInfo.AssemblyList != null)
                            {


                                foreach (var assembly in projectInfoModel.AssemblyInfo.AssemblyList)
                                {
                                    string guid = Guid.NewGuid().ToString();
                                    foreach (var item in projectInfoModel.BaseInfo.LanguageList)
                                    {
                                        assembly.SectionName = guid;
                                        list.Add($"LangString {assembly.SectionName} {item.LanguageDisplayKey} \"{GetLanguage(assembly.AssemblyName, item.LanguageType)}\"");
                                    }
                                }
                            }
                        }
                        foreach (var item in projectInfoModel.BaseInfo.LanguageList)
                        {
                            list.Add($"LangString ApplicationPublisher {item.LanguageDisplayKey} \"{GetLanguage(projectInfoModel.BaseInfo?.ApplicationPublisher, item.LanguageType)}\"");
                            list.Add($"LangString CompanyName {item.LanguageDisplayKey} \"{GetLanguage(projectInfoModel.BaseInfo?.CompanyName, item.LanguageType)}\"");
                            if (projectInfoModel.FinishInfo != null)
                            {
                                if (!string.IsNullOrWhiteSpace(projectInfoModel.FinishInfo.UninstallTip))
                                {
                                    list.Add($"LangString UninstallTip {item.LanguageDisplayKey} \"{GetLanguage(projectInfoModel.FinishInfo.UninstallTip, item.LanguageType)}\"");
                                }
                                if (!string.IsNullOrWhiteSpace(projectInfoModel.FinishInfo.UninstallMsg))
                                {
                                    list.Add($"LangString UninstallMsg {item.LanguageDisplayKey} \"{GetLanguage(projectInfoModel.FinishInfo.UninstallMsg, item.LanguageType)}\"");
                                }
                                if (!string.IsNullOrWhiteSpace(projectInfoModel.FinishInfo.UninstallProcessTips))
                                {
                                    list.Add($"LangString UninstallProcessTips {item.LanguageDisplayKey} \"{GetLanguage(projectInfoModel.FinishInfo.UninstallProcessTips, item.LanguageType)}\"");
                                }
                                if (!string.IsNullOrWhiteSpace(projectInfoModel.FinishInfo.InstallProcessTips))
                                {
                                    list.Add($"LangString InstallProcessTips {item.LanguageDisplayKey} \"{GetLanguage(projectInfoModel.FinishInfo.InstallProcessTips, item.LanguageType)}\"");
                                }
                            }
                        }

                    }
                    list.Add(";多语言结束");
                    if (!string.IsNullOrWhiteSpace(projectInfoModel.BaseInfo.ProductVersion))
                        list.Add(" VIProductVersion \"${PRODUCT_VERSION_A}\" ;");
                    list.Add($" VIAddVersionKey  \"ProductName\" \"${{PRODUCT_NAME}}\"");
                    list.Add($" VIAddVersionKey  \"FileVersion\" \"${{PRODUCT_VERSION}}\"");
                    list.Add($" VIAddVersionKey  \"ProductVersion\" \"${{PRODUCT_VERSION}}\"");
                    if (!projectInfoModel.BaseInfo.IsShowInUnInstall)
                    {
                        list.Add("Name \"${PRODUCT_NAME}\"");
                    }
                    else
                    {
                        list.Add("Name \"${PRODUCT_NAME} ${PRODUCT_VERSION}\"");
                    }
                    string outPath = Path.Combine(baseInfo.WorkSpace, "OutPut");
                    if (!Directory.Exists(outPath))
                    {
                        Directory.CreateDirectory(outPath);
                    }
                    var filePath = Path.Combine(outPath, baseInfo.AppOutPath.Replace(".exe", "") + ".exe");
                    if (projectInfoModel.FinishInfo.IsEnableProcess == true && projectInfoModel.BaseInfo.AppOutPath.Replace(".exe", "").ToLower() == projectInfoModel.FinishInfo.ProcessName.Replace(".exe", "").ToLower())
                    {
                        filePath = Path.Combine(outPath, baseInfo.AppOutPath.Replace(".exe", "") + " V" + baseInfo.ApplicationVersion + ".exe");
                    }
                    OutPutFilePath = filePath;
                    list.Add($"OutFile \"{filePath}\"");
                    var path = baseInfo.UserDirPath;
                    if (string.IsNullOrWhiteSpace(path))
                        path = baseInfo.ApplicationName;
                    list.Add($"InstallDir \"{baseInfo.SystemDir}\\{path}\"");
                    list.Add($"InstallDirRegKey HKLM \"${{PRODUCT_DIR_REGKEY}}\" \"\"");
                    list.Add("ShowInstDetails show");
                    list.Add("ShowUnInstDetails show");
                    if (!string.IsNullOrWhiteSpace(projectInfoModel.BaseInfo?.ApplicationPublisher))
                    {
                        list.Add($"  BrandingText \"{projectInfoModel.BaseInfo?.ApplicationPublisher}\"");
                    }
                    else
                    {
                        list.Add($"  BrandingText \"PackageEasy\"");
                    }

                }

                list.Add("Function .onInit");
                list.Add("  !insertmacro MUI_LANGDLL_DISPLAY");

                //检测进程
                if (projectInfoModel.FinishInfo != null && projectInfoModel.FinishInfo.IsEnableProcess)
                {
                    list.Add($"  nsProcess::_FindProcess \"{projectInfoModel.FinishInfo.ProcessName}\"");
                    list.Add("  Pop $R0");
                    list.Add("  IntCmp $R0 0 running no_running no_running");
                    list.Add("  running:");
                    if (projectInfoModel.BaseInfo.LanguageList != null && projectInfoModel.BaseInfo.LanguageList.Count > 0)
                    {
                        bool isFirst = true;
                        foreach (var lang in projectInfoModel.BaseInfo.LanguageList)
                        {
                            if (isFirst)
                            {
                                isFirst = false;
                                list.Add("  ${If} $LANGUAGE == " + lang.LanguageDisplayKey);

                            }
                            else
                            {
                                list.Add("  ${ElseIf} $LANGUAGE ==" + lang.LanguageDisplayKey);
                            }
                            list.Add($"  MessageBox MB_ICONQUESTION|MB_YESNO \"{GetLanguage(projectInfoModel.FinishInfo.InstallProcessTips, lang.LanguageType)}\" IDYES dokill IDNO stopit");
                        }
                        list.Add("  ${EndIf}");
                    }
                    else
                    {
                        list.Add("  MessageBox MB_ICONQUESTION|MB_YESNO \"$(InstallProcessTips)\" IDYES dokill IDNO stopit");
                    }
                    list.Add("  no_running:");
                    list.Add("  GoTo endding");
                    list.Add("  dokill:");
                    list.Add($"  nsProcess::_CloseProcess \"{projectInfoModel.FinishInfo.ProcessName}\"");
                    list.Add("  Pop $R0");
                    list.Add("  GoTo endding");
                    list.Add("  stopit:");
                    list.Add("  Abort");
                    list.Add("  endding:");
                    list.Add("  nsProcess::_Unload");
                }
                list.Add("FunctionEnd");
                List<string> sections = new List<string>();
                Dictionary<string, string> sectionDic = new Dictionary<string, string>();
                List<string> delDirs = new List<string>();
                string currentDirectory = "";
                bool hasSetIcon = false;
                int index = 1;
                if (projectInfoModel.AssemblyInfo != null)
                {
                    if (projectInfoModel.AssemblyInfo.AssemblyList != null)
                    {


                        foreach (var item in projectInfoModel.AssemblyInfo.AssemblyList)
                        {
                            string sec = $"SEC{index}";
                            string str = $"Section \"$({item.SectionName})\" {sec}";
                            list.Add(str);
                            list.Add("  SetOverwrite try");
                            sections.Add(sec);
                            if (!string.IsNullOrWhiteSpace(item.AssemblyDescription))
                                sectionDic.Add(sec, item.AssemblyDescription);
                            foreach (var file in item.FileList)
                            {
                                string dirPath = file.TargetPath.DisplayName;
                                if (!string.IsNullOrWhiteSpace(file.SubPath))
                                    dirPath += file.SubPath;
                                if (currentDirectory != dirPath)
                                {
                                    delDirs.Add(dirPath);
                                    currentDirectory = dirPath;
                                    list.Add($"  SetOutPath \"{dirPath}\"");
                                }
                                var path = projectInfoModel?.BaseInfo?.WorkSpace + file.FilePath;
                                if (!file.IsDirectory)
                                    //    list.Add($"  CreateDirectory  \"{path}\"");
                                    //else
                                    list.Add($"  File \"{path}\"");
                                FileInfo fileInfo = new FileInfo(path);
                                if (file.IsNeedInstall)
                                {
                                    list.Add($"  ExecWait '\"$INSTDIR\\{fileInfo.Name}\"'");
                                }
                                if (file.IsNeedQuietInstall)
                                {
                                    list.Add($"  ExecWait '\"$INSTDIR\\{fileInfo.Name}\" /q'");
                                }
                            }
                            if (!hasSetIcon)
                            {
                                hasSetIcon = true;
                                list.Add(" ${GetSize} \"$INSTDIR\" \"/S=0K\" $0 $1 $2");
                                list.Add(" IntFmt $0 \"0x%08X\" $0");
                                list.Add("  WriteRegDWORD ${PRODUCT_UNINST_ROOT_KEY} \"${PRODUCT_UNINST_KEY}\" \"EstimatedSize\" \"$0\"");
                                if (projectInfoModel.AppIcon != null)
                                {
                                    string dirName = projectInfoModel?.BaseInfo?.ApplicationName;
                                    if (projectInfoModel.AppIcon.IsCanChangeStartMenuName)
                                    {
                                        list.Add($"  !insertmacro MUI_STARTMENU_WRITE_BEGIN Application");
                                        dirName = "$ICONS_GROUP";
                                    }
                                    if (projectInfoModel.AppIcon.AppIconInfoList != null)
                                    {
                                        if (projectInfoModel.AppIcon.AppIconInfoList.Any(c => c.IconDir.Data == TargetDirType.SMPROGRAMS))
                                            list.Add($"  CreateDirectory \"$SMPROGRAMS\\{dirName ?? "a"}\"");

                                        foreach (var icon in projectInfoModel.AppIcon.AppIconInfoList)
                                        {
                                            if (icon.IconDir.Data == TargetDirType.SMPROGRAMS)
                                            {
                                                list.Add($"  CreateShortCut \"$SMPROGRAMS\\{dirName ?? "a"}\\{icon.ShortcutPath ?? "a"}.lnk\" \"{icon.FilePath}\"");
                                            }
                                            else
                                            {
                                                list.Add($"  CreateShortCut \"{icon.IconDir.DisplayName ?? "a"}\\{icon.ShortcutPath ?? "a"}.lnk\" \"{icon.FilePath}\"");
                                            }
                                        }
                                    }
                                    if (projectInfoModel.AppIcon.IsCanChangeStartMenuName)
                                    {
                                        list.Add($"  !insertmacro MUI_STARTMENU_WRITE_END");
                                    }
                                }

                                if (projectInfoModel?.Registry != null)
                                {
                                    if (!string.IsNullOrWhiteSpace(projectInfoModel.Registry?.RegistryFormat) && projectInfoModel.Registry?.IsAsSelected == false)
                                    {
                                        var allFormats = projectInfoModel.Registry?.RegistryFormat.Split(',').ToList();
                                        if (allFormats != null && allFormats.Count > 0)
                                        {
                                            foreach (var format in allFormats)
                                            {
                                                index++;
                                                var strFormat = format;
                                                if (!format.StartsWith('.'))
                                                    strFormat = "." + format;
                                                var fileName = strFormat.Replace(".", "");


                                                list.Add($" WriteRegStr HKCR \"{strFormat}\" \"\" \"{fileName}File\"");
                                                list.Add($" WriteRegStr HKCR \"{fileName}File\" \"\" \"{fileName}File data file\"");
                                                list.Add($" WriteRegStr HKCR \"{fileName}File\\DefaultIcon\" \"\" \"$INSTDIR\\{projectInfoModel?.Registry?.ProcessName},0\"");
                                                list.Add($" WriteRegStr HKCR \"{fileName}File\\shell\" \"\" \"\"");
                                                list.Add($" WriteRegStr HKCR \"{fileName}File\\shell\\open\" \"\" \"\"");
                                                list.Add($" WriteRegStr HKCR \"{fileName}File\\shell\\open\\command\" \"\" '\"$INSTDIR\\{projectInfoModel?.Registry?.ProcessName}\" -o \"%1\"'");

                                            }
                                            list.Add("!include \"FileFunc.nsh\"");
                                            list.Add("${RefreshShellIcons}");
                                        }
                                    }
                                }
                            }
                            list.Add("SectionEnd");
                            index++;
                        }
                    }
                }
                if (projectInfoModel?.Registry != null)
                {
                    if (!string.IsNullOrWhiteSpace(projectInfoModel.Registry?.RegistryFormat) && projectInfoModel.Registry?.IsAsSelected == true)
                    {
                        var allFormats = projectInfoModel.Registry?.RegistryFormat.Split(',').ToList();
                        if (allFormats != null && allFormats.Count > 0)
                        {
                            foreach (var format in allFormats)
                            {
                                index++;
                                var str = format;
                                if (!format.StartsWith('.'))
                                    str = "." + format;
                                var fileName = str.Replace(".", "");
                                string sec = $"SEC{index}";
                                sections.Add(sec);

                                string strSection = $"Section \"{str}\" {sec}";
                                list.Add(strSection);
                                list.Add($" WriteRegStr HKCR \"{str}\" \"\" \"{fileName}File\"");
                                list.Add($" WriteRegStr HKCR \"{fileName}File\" \"\" \"{fileName}File data file\"");
                                list.Add($" WriteRegStr HKCR \"{fileName}File\\DefaultIcon\" \"\" \"$INSTDIR\\{projectInfoModel?.Registry?.ProcessName},0\"");
                                list.Add($" WriteRegStr HKCR \"{fileName}File\\shell\" \"\" \"\"");
                                list.Add($" WriteRegStr HKCR \"{fileName}File\\shell\\open\" \"\" \"\"");
                                list.Add($" WriteRegStr HKCR \"{fileName}File\\shell\\open\\command\" \"\" '\"$INSTDIR\\{projectInfoModel?.Registry?.ProcessName}\" -o \"%1\"'");
                                list.Add("!include \"FileFunc.nsh\"");
                                list.Add("${RefreshShellIcons}");
                                list.Add("SectionEnd");
                            }
                        }
                    }
                }
                list.Add(";添加卸载图标");
                list.Add("Section -AdditionalIcons");
                string startMenuName = projectInfoModel?.AppIcon?.StartMenuName;
                if (projectInfoModel.AppIcon != null && projectInfoModel.AppIcon.IsCanChangeStartMenuName)
                {
                    list.Add($"!insertmacro MUI_STARTMENU_WRITE_BEGIN Application");
                    startMenuName = "$ICONS_GROUP";
                }
                else
                {
                    list.Add($"  CreateDirectory \"$SMPROGRAMS\\{startMenuName ?? "a"}\"");
                }
                if (!string.IsNullOrEmpty(startMenuName))
                {
                    delDirs.Add(startMenuName);
                }
                list.Add("   WriteIniStr \"$INSTDIR\\${PRODUCT_NAME}.url\" \"InternetShortcut\" \"URL\" \"${PRODUCT_WEB_SITE}\"");
                if (projectInfoModel?.AppIcon?.IsCreateWebUrl == true)
                    list.Add($"  CreateShortCut \"$SMPROGRAMS\\{startMenuName}\\Website.lnk\" \"$INSTDIR\\${{PRODUCT_NAME}}.url\"");
                if (projectInfoModel?.AppIcon?.IsCreateUnInstall == true)
                    list.Add($"  CreateShortCut \"$SMPROGRAMS\\{startMenuName}\\Uninstall.lnk\" \"$INSTDIR\\uninst.exe\"");
                if (projectInfoModel.AppIcon != null && projectInfoModel.AppIcon.IsCanChangeStartMenuName)
                {
                    list.Add($"!insertmacro MUI_STARTMENU_WRITE_END");
                }
                list.Add("SectionEnd");
                list.Add("Section -Post");
                list.Add($"  WriteUninstaller \"$INSTDIR\\uninst.exe\"");
                list.Add($"  WriteRegStr HKLM \"${{PRODUCT_DIR_REGKEY}}\" \"\" \"{projectInfoModel?.FinishInfo?.ApplicationName}\"");
                list.Add("  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} \"${PRODUCT_UNINST_KEY}\" \"DisplayName\" \"$(^Name)\"");
                list.Add("  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} \"${PRODUCT_UNINST_KEY}\" \"UninstallString\" \"$INSTDIR\\uninst.exe\"");
                list.Add($"  WriteRegStr ${{PRODUCT_UNINST_ROOT_KEY}} \"${{PRODUCT_UNINST_KEY}}\" \"DisplayIcon\" \"{projectInfoModel?.FinishInfo?.ApplicationName}\"");
                list.Add("  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} \"${PRODUCT_UNINST_KEY}\" \"DisplayVersion\" \"${PRODUCT_VERSION}\"");
                list.Add("  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} \"${PRODUCT_UNINST_KEY}\" \"URLInfoAbout\" \"${PRODUCT_WEB_SITE}\"");
                list.Add("  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} \"${PRODUCT_UNINST_KEY}\" \"Publisher\" \"${PRODUCT_PUBLISHER}\"");

                list.Add("SectionEnd");
                if (sectionDic != null && sectionDic.Count > 0)
                {
                    if (projectInfoModel.BaseInfo.LanguageList != null)
                    {
                        if (projectInfoModel.AssemblyInfo != null)
                        {
                            if (projectInfoModel.AssemblyInfo.AssemblyList != null)
                            {


                                foreach (var section in sectionDic)
                                {
                                    foreach (var item in projectInfoModel.BaseInfo.LanguageList)
                                    {
                                        list.Add($"LangString {section.Key} {item.LanguageDisplayKey} \"{GetLanguage(section.Value, item.LanguageType)}\"");
                                    }
                                }
                            }
                        }

                    }
                }
                list.Add("!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN");
                foreach (var item in sections)
                {
                    string desc = sectionDic.ContainsKey(item) ? sectionDic[item] : "";
                    list.Add($"  !insertmacro MUI_DESCRIPTION_TEXT ${{{item}}} \"$({item})\"");
                }
                list.Add("!insertmacro MUI_FUNCTION_DESCRIPTION_END");
                list.Add("Function un.onUninstSuccess");
                list.Add("  HideWindow");
                if (projectInfoModel.FinishInfo != null)
                {
                    if (!string.IsNullOrWhiteSpace(projectInfoModel.FinishInfo.UninstallMsg))
                    {
                        list.Add($"  MessageBox MB_ICONINFORMATION|MB_OK \"$(UninstallMsg)\"");
                    }
                }

                list.Add("FunctionEnd");

                list.Add("Function un.onInit");
                list.Add(" !insertmacro MUI_UNGETLANGUAGE");
                if (projectInfoModel.FinishInfo != null)
                {
                    if (!string.IsNullOrWhiteSpace(projectInfoModel.FinishInfo.UninstallTip))
                    {

                        if (projectInfoModel.BaseInfo.LanguageList != null && projectInfoModel.BaseInfo.LanguageList.Count > 0)
                        {
                            bool isFirst = true;
                            foreach (var lang in projectInfoModel.BaseInfo.LanguageList)
                            {
                                if (isFirst)
                                {
                                    isFirst = false;
                                    list.Add("  ${If} $LANGUAGE == " + lang.LanguageDisplayKey);

                                }
                                else
                                {
                                    list.Add("  ${ElseIf} $LANGUAGE ==" + lang.LanguageDisplayKey);
                                }
                                list.Add($" MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 \"{GetLanguage(projectInfoModel.FinishInfo.UninstallTip, lang.LanguageType)}\" IDYES +2");
                                //list.Add($"  MessageBox MB_ICONQUESTION|MB_YESNO \"{GetLanguage(projectInfoModel.FinishInfo.UninstallProcessTips, lang.LanguageType)}\" IDYES dokill IDNO stopit");
                            }
                            list.Add("  ${EndIf}");
                        }
                        else
                        {
                            list.Add($" MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 \"$(UninstallTip)\" IDYES +2");
                        }
                        list.Add("  Abort");
                    }
                }
                if (projectInfoModel.FinishInfo != null && projectInfoModel.FinishInfo.IsEnableProcess)
                {
                    list.Add($"  nsProcess::_FindProcess \"{projectInfoModel.FinishInfo.ProcessName}\"");
                    list.Add("  Pop $R0");
                    list.Add("  IntCmp $R0 0 running no_running no_running");
                    list.Add("  running:");

                    if (projectInfoModel.BaseInfo.LanguageList != null && projectInfoModel.BaseInfo.LanguageList.Count > 0)
                    {
                        bool isFirst = true;
                        foreach (var lang in projectInfoModel.BaseInfo.LanguageList)
                        {
                            if (isFirst)
                            {
                                isFirst = false;
                                list.Add("  ${If} $LANGUAGE == " + lang.LanguageDisplayKey);

                            }
                            else
                            {
                                list.Add("  ${ElseIf} $LANGUAGE ==" + lang.LanguageDisplayKey);
                            }
                            list.Add($"  MessageBox MB_ICONQUESTION|MB_YESNO \"{GetLanguage(projectInfoModel.FinishInfo.UninstallProcessTips, lang.LanguageType)}\" IDYES dokill IDNO stopit");
                        }
                        list.Add("  ${EndIf}");
                    }
                    else
                    {
                        list.Add("  MessageBox MB_ICONQUESTION|MB_YESNO \"$(UninstallProcessTips)\" IDYES dokill IDNO stopit");
                    }
                    //list.Add("  MessageBox MB_ICONQUESTION|MB_YESNO \"$(UninstallProcessTips)\" IDYES dokill IDNO stopit");
                    list.Add("  no_running:");
                    list.Add("  GoTo endding");
                    list.Add("  dokill:");
                    list.Add($"  nsProcess::_CloseProcess \"{projectInfoModel.FinishInfo.ProcessName}\"");
                    list.Add("  Pop $R0");
                    list.Add("  GoTo endding");
                    list.Add("  stopit:");
                    list.Add("  Abort");
                    list.Add("  endding:");
                    list.Add("  nsProcess::_Unload");
                }
                list.Add("FunctionEnd");
                list.Add(";卸载开始");
                list.Add("Section Uninstall");
                if (projectInfoModel != null && projectInfoModel.AppIcon != null && projectInfoModel.AppIcon.IsCanChangeStartMenuName)
                {
                    list.Add($"!insertmacro MUI_STARTMENU_GETFOLDER \"Application\" $ICONS_GROUP");
                }
                if (projectInfoModel != null && projectInfoModel.AssemblyInfo != null)
                {
                    if (projectInfoModel.AssemblyInfo.AssemblyList != null)
                    {

                        int count = 1;
                        foreach (var item in projectInfoModel.AssemblyInfo.AssemblyList)
                        {

                            foreach (var file in item.FileList)
                            {

                                list.Add($" Delete \"{file.TargetPath.DisplayName}{file.FilePath}\"");

                            }
                            count++;
                        }
                    }
                }

                if (projectInfoModel != null && projectInfoModel.Registry != null)
                {
                    if (!string.IsNullOrWhiteSpace(projectInfoModel.Registry?.RegistryFormat))
                    {
                        var allFormats = projectInfoModel.Registry?.RegistryFormat.Split(',').ToList();
                        if (allFormats != null && allFormats.Count > 0)
                        {
                            foreach (var format in allFormats)
                            {
                                var str = format;
                                if (!format.StartsWith('.'))
                                    str = "." + format;
                                var fileName = str.Replace(".", "");
                                list.Add($" DeleteRegKey HKCR \"{str}\" \"\" \"{fileName}File\"");
                                list.Add($" DeleteRegKey HKCR \"{fileName}File\" \"\" \"{fileName}File data file\"");
                                list.Add($" DeleteRegKey HKCR \"{fileName}File\\DefaultIcon\" \"\" \"$INSTDIR\\{projectInfoModel?.Registry?.ProcessName},0\"");
                                list.Add($" DeleteRegKey HKCR \"{fileName}File\\shell\" \"\" \"\"");
                                list.Add($" DeleteRegKey HKCR \"{fileName}File\\shell\\open\" \"\" \"\"");
                                list.Add($" DeleteRegKey HKCR \"{fileName}File\\shell\\open\\command\" \"\" '\"$INSTDIR\\{projectInfoModel?.Registry?.ProcessName}\" -o \"%1\"'");
                                if (projectInfoModel.Registry?.IsAsSelected == true)
                                {
                                    list.Add("!include \"FileFunc.nsh\"");
                                    list.Add("${RefreshShellIcons}");
                                }
                            }
                        }
                    }
                }

                if (hasSetIcon)
                {
                    if (projectInfoModel != null && projectInfoModel.Registry != null && projectInfoModel.Registry?.IsAsSelected == false)
                    {
                        list.Add("!include \"FileFunc.nsh\"");
                        list.Add("${RefreshShellIcons}");
                    }
                    if (projectInfoModel != null && projectInfoModel.AppIcon != null)
                    {
                        if (projectInfoModel.AppIcon.AppIconInfoList != null)
                        {
                            string dirPath = projectInfoModel?.BaseInfo?.ApplicationName;
                            if (projectInfoModel.AppIcon.IsCanChangeStartMenuName)
                            {
                                dirPath = "$ICONS_GROUP";
                            }
                            foreach (var icon in projectInfoModel.AppIcon.AppIconInfoList)
                            {
                                if (icon.IconDir.Data == TargetDirType.SMPROGRAMS)
                                {
                                    list.Add($" Delete \"$SMPROGRAMS\\{dirPath ?? "a"}\\{icon.ShortcutPath ?? "a"}.lnk\"");
                                }
                                else
                                {
                                    list.Add($" Delete \"{icon.IconDir.DisplayName ?? "a"}\\{icon.ShortcutPath ?? "a"}.lnk\"");
                                }
                            }
                            if (projectInfoModel.AppIcon.AppIconInfoList.Any(c => c.IconDir.Data == TargetDirType.SMPROGRAMS))
                                list.Add($" RMDir /r \"$SMPROGRAMS\\{dirPath ?? "a"}\"");
                        }
                    }
                }

                delDirs = delDirs.OrderByDescending(x => x.Length).ToList();
                foreach (var item in delDirs)
                {
                    string dir = $" RMDir /r  \"{item}\"";
                    if (list.Exists(f => f == dir)) continue;
                    list.Add(dir);
                }
                list.Add($" Delete \"$INSTDIR\\${{PRODUCT_NAME}}.url\"");//
                list.Add($" Delete \"$INSTDIR\\uninst.exe\"");
                list.Add($" DeleteRegKey ${{PRODUCT_UNINST_ROOT_KEY}} \"${{PRODUCT_UNINST_KEY}}\"");
                list.Add($" DeleteRegKey HKLM \"${{PRODUCT_DIR_REGKEY}}\"");
                list.Add("SectionEnd");



            }

            return list;
        }

        public override Task<List<string>> BuildAsync(ProjectInfoModel projectInfoModel, ViewType viewType)
        {
            throw new NotImplementedException();
        }

        public override Task<List<string>> BuildAsync(ProjectInfoModel projectInfoModel)
        {
            throw new NotImplementedException();
        }



        public override string GetParams<T>(T t)
        {
            if (t == null) return "";
            string? str = t as string;
            if (!string.IsNullOrWhiteSpace(str))
            {
                str = $"/INPUTCHARSET UTF8 \"{str}\"";
            }
            return str ?? "";

        }
    }
}
