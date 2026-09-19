const ICONS: Record<string, string> = {
  battery: "\u{1F50B}",
  ev: "\u{1F50C}",
  heatpump: "\u{2668}\u{FE0F}",
  solar: "\u{2600}\u{FE0F}",
};

export function AssetIcon({ assetType }: { assetType?: string }) {
  return <div className="asset-icon">{ICONS[assetType ?? ""] ?? "\u{26A1}"}</div>;
}
