!include "MUI2.nsh"
!include "FileFunc.nsh"   ; for ${GetSize} for EstimatedSize registry entry

# Author: Dr. H. Maerz, 22.11.22 

# NSIS script for creating the setup bootstrapper for the categorizer.
# All files of the dedicated directory in BUILDIR will be sampled and compressed to a
# setup.exe and an uninstall.exe. Both files will be digitally signed automatically,
# so both procedures are accepted by the Windows OS.
# While the installation of the categorizer files is mandatory, the creation of a
# start menu or/and desktop link are optional. By default, program files are installed
# to the default program directory %PROGRAMS% but selecting another directory is an option, too.

# PREREQUISITES:
# - NSIS 3.08  ->https://nsis.sourceforge.io/Main_Page
# - HM NSIS Editor ->http://hmne.sourceforge.net/

# USAGE:
# - edit the defines to your needs (Version, Builddir)
# - save nsi file
# - compile
# - distribute

Name "PhotoTufinSetup"

# EDIT TO YOUR NEEDS
!define APPNAME "PhotoTupledFinder"
!define SHORTNAME "PhotoTufin"
!define COMPANYNAME "McGerhard Photography"
!define VERSION "1.0.33.328"
!define PRODUCTNAME "Photo Tufin"
!define PRODUCTVERSION "1.0" # Only 2 digits!
!define BUILDDIR "C:\Users\Maerz\Projekte\PhotoTupletFinder\PhotoTufin\PhotoTufin\bin\Publish" #Directory of the build
# EDIT END

OutFile "${SHORTNAME}-${VERSION}-setup.exe"

Caption "${APPNAME} v${VERSION} Setup"
UninstallCaption "Uninstall ${APPNAME} v${VERSION}"
BrandingText "Copyright � ${COMPANYNAME}. All rights reserved."

# installer options RTFM
SetDateSave on
SetDatablockOptimize on
CRCCheck on
SilentInstall normal
BGGradient 000000 003780 FFFFFF
InstallColors FF8080 000030
XPStyle on

InstallDir "$PROGRAMFILES\${COMPANYNAME}\${APPNAME}"

#!define MUI_WELCOMEFINISHPAGE_BITMAP "${NSISDIR}\Contrib\Graphics\Wizard\orange.bmp"
#!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_LANGUAGE "German"

# descriptions of optional components
LangString DESC_DESKTOP ${LANG_GERMAN} "Erstellt eine Verkn�pfung auf dem Desktop"
LangString DESC_START_MENU ${LANG_GERMAN} "Erstellt Eintr�ge im Startmenu"

;--------------------------------
;Version Information

  VIProductVersion "${PRODUCTVERSION}.0.0"
  VIFileVersion "${VERSION}"
  # setup installer details
  VIAddVersionKey /LANG=${LANG_GERMAN} "ProductName" "${PRODUCTNAME}"
  VIAddVersionKey /LANG=${LANG_GERMAN} "ProductVersion" "${PRODUCTVERSION}"
  VIAddVersionKey /LANG=${LANG_GERMAN} "Comments" "Only for internal use"
  VIAddVersionKey /LANG=${LANG_GERMAN} "CompanyName" "${COMPANYNAME}"
  VIAddVersionKey /LANG=${LANG_GERMAN} "LegalCopyright" "Copyright � ${COMPANYNAME}. All rights reserved."
  VIAddVersionKey /LANG=${LANG_GERMAN} "FileDescription" "${PRODUCTNAME} Setup Bootstrapper"
  VIAddVersionKey /LANG=${LANG_GERMAN} "FileVersion" "${VERSION}"

;--------------------------------

Section "Desktop Verkn�pfung" SectionDesktop
  ;Desktop link
  CreateShortCut "$DESKTOP\${APPNAME}.lnk" "$INSTDIR\${SHORTNAME}.exe"
SectionEnd

Section "Start Menu Eintrag" SectionStartMenu
  CreateDirectory "$SMPROGRAMS\${COMPANYNAME}"
  CreateShortCut "$SMPROGRAMS\${COMPANYNAME}\Uninstall ${APPNAME}.lnk" "$INSTDIR\uninstall.exe" "$INSTDIR\uninstall.exe"
  CreateShortCut "$SMPROGRAMS\${COMPANYNAME}\${APPNAME}.lnk" "$INSTDIR\${SHORTNAME}.exe" "$INSTDIR\${SHORTNAME}.exe"
SectionEnd

Section ${APPNAME}
  SetOutPath $INSTDIR
  SectionIn RO
  # files to install
  File "${BUILDDIR}\*.exe"
  File "${BUILDDIR}\*.dll"
  File /nonfatal "${BUILDDIR}\*.config"
  File /nonfatal "${BUILDDIR}\*.nfo"

  ; get cumulative size of all files in and under install dir
  ; report the total in KB (decimal)
  ; place the answer into $0  (ignore $1 $2)
  ${GetSize} "$INSTDIR" "/S=0K" $0 $1 $2

  ; Convert the decimal KB value in $0 to DWORD
  ; put it right back into $0
  IntFmt $0 "0x%08X" $0

  ; Create/Write the reg key with the dword value
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "EstimatedSize" "$0"

  # uninstaller
  WriteUninstaller $INSTDIR\uninstall.exe
  # System app install information
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "DisplayName" "${APPNAME}"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "DisplayVersion" "${VERSION}"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "Publisher" "${COMPANYNAME}"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "DisplayIcon" "$INSTDIR\PhotoTufin.exe"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "UninstallString" '"$INSTDIR\uninstall.exe"'
  # There is no option for modifying or repairing the install
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "NoRepair" 1

SectionEnd


# macros have to be defined after sections
!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
!insertmacro MUI_DESCRIPTION_TEXT ${SectionDesktop} $(DESC_DESKTOP)
!insertmacro MUI_DESCRIPTION_TEXT ${SectionStartMenu} $(DESC_START_MENU)
!insertmacro MUI_FUNCTION_DESCRIPTION_END

Section "Uninstall"
        # Remove desktop launcher
	delete "$DESKTOP\${APPNAME}.lnk"

        # Remove Start Menu launcher
	delete "$SMPROGRAMS\${COMPANYNAME}\${APPNAME}.lnk"
	delete "$SMPROGRAMS\${COMPANYNAME}\Uninstall ${APPNAME}.lnk"
	rmDir "$SMPROGRAMS\${COMPANYNAME}"

	# Remove files
	delete $INSTDIR\${SHORTNAME}.exe
	delete $INSTDIR\*.dll
	delete $INSTDIR\*.config
	delete $INSTDIR\*.nfo
	delete $INSTDIR\*.lnk

	# Always delete uninstaller as the last action
	delete $INSTDIR\uninstall.exe

	# Try to remove the install directory - this will only happen if it is empty
	rmDir $INSTDIR

	# Remove uninstaller information from the registry
	DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}"
SectionEnd