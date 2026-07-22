<hr />
<p>&nbsp;</p>
<p><img style="display: block; margin-left: auto; margin-right: auto;" src="https://github.com/user-attachments/assets/614be4e9-8aa2-4b80-8717-54ce0c128c68" alt="" width="777" height="437" /></p>
<h4 style="text-align: center;"><em>Do you want more diverse molds while staying true to the spirit and mechanics of the game? This mod is for you!</em></h4>
<p>&nbsp;</p>
<h4 style="text-align: center;"><strong>The VanillaMoreMolds project aims to add new varieties of molds to expand the arsenal of your forge, while staying true to the base game mechanics, smelting logic, and crafting style.</strong></h4>
<h4 style="text-align: center;"><strong>The mod also introduces a brand-new mechanic for certain molds, designed to stay as close as possible to what the base game could naturally offer.</strong></h4>
<h2>&nbsp;</h2>
<p>Let's talk a bit about <strong>v2.1.0</strong>! This release came much faster than I expected.</p>
<p>I've started refactoring several of my C# classes to make many systems less hard-coded and more JSON-driven. This made it much easier to implement the new <strong>Heavy Mold (Rod)</strong> and will also make future additions much easier to maintain.</p>
<p>Technically, it's already possible to create addons, but these changes are mainly intended to let me develop more interesting updates in the future. To make addon development easier for others, I'll need to provide proper documentation, which isn't ready yet. x)</p>
<p>&nbsp;</p>
<p>Your feedback has also been incredibly helpful in fixing configuration issues. Once again, if you run into any problems, please don't hesitate to let me know.</p>
<p>I've also added block remapping for the <strong>Heavy Mold (Ingot)</strong> and <strong>Heavy Mold (Plate)</strong>, since their asset paths changed between <strong>v2.0.1</strong> and <strong>v2.1.0</strong>. You shouldn't encounter any issues when updating, except that the sand color inside existing ingot and plate molds may change.</p>
<p>&nbsp;</p>
<h4><span style="color: #e03e2d;">I also have a question for you all :</span></h4>
<p>Right now, after using a Heavy Mold, the inprint in the sand remains and can be reused. Would you prefer it to reset after each use, so the mold becomes empty again?</p>
<h2>What&rsquo;s new in 2.1.2?</h2>
<h3>FEATURE :</h3>
<ul>
<li>Added&nbsp;<strong>Russian</strong> translation by ChimaMAG.</li>
</ul>
<h3>UPDATE :</h3>
<ul>
<li>
<p>Updated the supported game versions in modinfo to properly support Vintage Story&nbsp;<strong>1.22.4</strong>&nbsp;and&nbsp;<strong>1.22.5</strong>.</p>
</li>
</ul>
<h3>FIX :</h3>
<ul>
<li>Corrected the explanation text for the "Heavy Mold handbook info" entry in the <strong>en.json</strong> and <strong>fr.json</strong> files.</li>
</ul>
<h3>KNOW ISSUE :</h3>
<ul>
<li>If you break Heavy Molds by hand instead of retrieving them properly, the sand does not display the correct texture. <em>(This is a known bug the mold should appear empty, i'm gona fix this in the coming days.)</em></li>
</ul>
<h2 style="text-align: left;">Feature: Tool molds</h2>
<table style="border-collapse: collapse; width: 75.6267%; height: 124px; background-color: #95a5a6; border-color: #000000; border-style: solid;" border="3">
<tbody>
<tr style="height: 21px;">
<td style="width: 11.1055%; text-align: left; height: 21px;"><strong>Mold name:</strong></td>
<td style="width: 11.1055%; text-align: center; height: 21px;"><strong>Sawblade Mold</strong></td>
<td style="width: 11.1055%; text-align: center; height: 21px;"><strong>Nail and Strips Mold</strong></td>
<td style="width: 11.1055%; text-align: center; height: 21px;"><strong>Arrowhead Mold</strong></td>
<td style="width: 11.1055%; text-align: center; height: 21px;"><strong>Hoop Mold</strong></td>
<td style="width: 11.1055%; text-align: center; height: 21px;"><strong>Plate Mold</strong></td>
<td style="width: 11.1055%; text-align: center; height: 21px;"><strong>Spearhead Mold</strong></td>
<td style="width: 11.1055%; text-align: center; height: 21px;"><strong>Knifeblade Mold</strong></td>
<td style="width: 11.1055%; text-align: center; height: 21px;"><strong>Scythehead Mold</strong></td>
</tr>
<tr style="height: 20px;">
<td style="text-align: left; height: 20px; width: 11.1055%;"><strong>Molten metal required:</strong></td>
<td style="text-align: center; height: 20px; width: 11.1055%;">100</td>
<td style="text-align: center; height: 20px; width: 11.1055%;">75</td>
<td style="text-align: center; height: 20px; width: 11.1055%;">100</td>
<td style="text-align: center; height: 20px; width: 11.1055%;">100</td>
<td style="text-align: center; height: 20px; width: 11.1055%;">200</td>
<td style="text-align: center; height: 20px; width: 11.1055%;">100</td>
<td style="text-align: center; height: 20px; width: 11.1055%;">100</td>
<td style="text-align: center; height: 20px; width: 11.1055%;">100</td>
</tr>
<tr style="height: 20px;">
<td style="text-align: left; height: 20px; width: 11.1055%;"><strong>Drop:</strong></td>
<td style="text-align: center; height: 20px; width: 11.1055%;">x1 sawblade</td>
<td style="text-align: center; height: 20px; width: 11.1055%;">x3 metal nails and strips</td>
<td style="text-align: center; height: 20px; width: 11.1055%;">x8 arrowheads</td>
<td style="text-align: center; height: 20px; width: 11.1055%;">x1 hoop</td>
<td style="text-align: center; height: 20px; width: 11.1055%;">x1 metal plate</td>
<td style="text-align: center; height: 20px; width: 11.1055%;">x1 spearhead</td>
<td style="text-align: center; height: 20px; width: 11.1055%;">x1 knifeblade</td>
<td style="text-align: center; height: 20px; width: 11.1055%;">x1 scythehead</td>
</tr>
<tr style="height: 21px;">
<td style="text-align: left; height: 21px; width: 11.1055%;"><strong>Type of clay:</strong></td>
<td style="text-align: center; height: 21px; width: 11.1055%;"><em>Blue, Fire, Red</em></td>
<td style="text-align: center; height: 21px; width: 11.1055%;"><em>Blue, Fire, Red</em></td>
<td style="text-align: center; height: 21px; width: 11.1055%;"><em>Blue, Fire, Red</em></td>
<td style="text-align: center; height: 21px; width: 11.1055%;"><em>Blue, Fire, Red</em></td>
<td style="text-align: center; height: 21px; width: 11.1055%;"><em>Blue, Fire, Red</em></td>
<td style="text-align: center; height: 21px; width: 11.1055%;"><em>Blue, Fire, Red</em></td>
<td style="text-align: center; height: 21px; width: 11.1055%;"><em>Blue, Fire, Red</em></td>
<td style="text-align: center; height: 21px; width: 11.1055%;"><em>Blue, Fire, Red</em></td>
</tr>
<tr style="height: 21px;">
<td style="text-align: left; height: 21px; width: 11.1055%;"><strong>Active by default:</strong></td>
<td style="text-align: center; height: 21px; width: 11.1055%;"><span style="background-color: #e74c3c;"><em>✗ Disabled</em></span></td>
<td style="text-align: center; height: 21px; width: 11.1055%;"><span style="background-color: #27ae60;"><em>✓ Enabled</em></span></td>
<td style="text-align: center; height: 21px; width: 11.1055%;"><span style="background-color: #e74c3c;"><em>✗ Disabled</em></span></td>
<td style="text-align: center; height: 21px; width: 11.1055%;"><span style="background-color: #27ae60;"><em>✓ Enabled</em></span></td>
<td style="text-align: center; height: 21px; width: 11.1055%;"><span style="background-color: #27ae60;"><em>✓ Enabled</em></span></td>
<td style="text-align: center; height: 21px; width: 11.1055%;"><span style="background-color: #e74c3c;"><em>✗ Disabled</em></span></td>
<td style="text-align: center; height: 21px; width: 11.1055%;"><span style="background-color: #e74c3c;"><em>✗ Disabled</em></span></td>
<td style="text-align: center; height: 21px; width: 11.1055%;"><span style="background-color: #e74c3c;"><em>✗ Disabled</em></span></td>
</tr>
<tr style="height: 21px;">
<td style="text-align: left; height: 21px; width: 11.1055%;"><strong>Shatters on contact with water.</strong></td>
<td style="text-align: center; height: 21px; width: 11.1055%;"><em>Yes</em></td>
<td style="text-align: center; height: 21px; width: 11.1055%;"><em>Yes</em></td>
<td style="text-align: center; height: 21px; width: 11.1055%;"><em>Yes</em></td>
<td style="text-align: center; height: 21px; width: 11.1055%;"><em>Yes</em></td>
<td style="text-align: center; height: 21px; width: 11.1055%;"><em>Yes</em></td>
<td style="text-align: center; height: 21px; width: 11.1055%;"><em>Yes</em></td>
<td style="text-align: center; height: 21px; width: 11.1055%;"><em>Yes</em></td>
<td style="text-align: center; height: 21px; width: 11.1055%;"><em>Yes</em></td>
</tr>
</tbody>
</table>
<h2 style="text-align: left;">Feature: Heavy molds</h2>
<p><span style="color: #e74c3c;"><em><strong>⚠ Note for players upgrading from 1.x:</strong> The old Light, Medium, and Heavy Ingot Molds have been removed and replaced by the new Heavy Mold system introduced in 2.0. If you had large ingot molds in your world, they will no longer be valid. The new Heavy Molds offer the same mass-production purpose with a richer crafting process and two output variants (ingots or plates).</em></span></p>
<p><strong>Heavy molds are large two-handed molds built in two steps :</strong> first shape the Heavy Base Mold via <strong>clayforming</strong>, fire it in a kiln, then assemble it with <strong>rope</strong>, metal <strong>nails &amp; strips</strong>, and <strong>sticks</strong>&nbsp;to create the placeable Heavy Mold. Once placed, fill it with <strong>sand</strong>, then Shift + right-click with an <strong>ingot</strong> or a <strong>metal plate</strong> to choose the output variant before pouring the metal.</p>
<p>&nbsp;</p>
<table style="border-collapse: collapse; width: 32.879%; background-color: #95a5a6; border-color: #000000; border-style: solid; height: 158.938px;" border="3" cellpadding="1">
<tbody>
<tr style="height: 21.1875px;">
<td style="width: 30.0126%; text-align: left; height: 21.1875px;"><strong>Variant:</strong></td>
<td style="width: 22.6986%; text-align: center; height: 21.1875px;"><strong>Ingot Mold</strong></td>
<td style="width: 24.3399%; text-align: center; height: 21.1875px;"><strong>Plate Mold</strong></td>
<td style="width: 22.9489%; text-align: center; height: 21.1875px;"><strong>Rod Mold</strong></td>
</tr>
<tr style="height: 20.1875px;">
<td style="text-align: left; height: 20.1875px; width: 30.0126%;"><strong>Molten metal required:</strong></td>
<td style="text-align: center; height: 20.1875px; width: 22.6986%;">600</td>
<td style="text-align: center; height: 20.1875px; width: 24.3399%;">800</td>
<td style="text-align: center; width: 22.9489%; height: 20.1875px;">900</td>
</tr>
<tr style="height: 20.1875px;">
<td style="text-align: left; height: 20.1875px; width: 30.0126%;"><strong>Drop:</strong></td>
<td style="text-align: center; height: 20.1875px; width: 22.6986%;">x6 Ingots</td>
<td style="text-align: center; height: 20.1875px; width: 24.3399%;">x4 Metal plates</td>
<td style="text-align: center; width: 22.9489%; height: 20.1875px;">x9 Metal rod</td>
</tr>
<tr style="height: 21px;">
<td style="text-align: left; height: 21px; width: 30.0126%;"><strong>Base clay (clayforming):</strong></td>
<td style="text-align: center; height: 21px; width: 22.6986%;"><em>Blue, Fire, Red</em></td>
<td style="text-align: center; height: 21px; width: 24.3399%;"><em>Blue, Fire, Red</em></td>
<td style="text-align: center; width: 22.9489%; height: 21px;"><em>Blue, Fire, Red</em></td>
</tr>
<tr style="height: 36px;">
<td style="text-align: left; height: 36px; width: 30.0126%;"><strong>Active by default:</strong></td>
<td style="text-align: center; height: 36px; width: 22.6986%;"><span style="background-color: #27ae60;"><em>✓ Enabled</em></span></td>
<td style="text-align: center; height: 36px; width: 24.3399%;"><span style="background-color: #27ae60;"><em>✓ Enabled</em></span></td>
<td style="text-align: center; width: 22.9489%; height: 36px;"><span style="background-color: #27ae60;"><em><span style="background-color: #27ae60;">✓ Enabled</span></em></span></td>
</tr>
<tr style="height: 40.375px;">
<td style="text-align: left; height: 40.375px; width: 30.0126%;"><strong>Shatters on contact with water.</strong></td>
<td style="text-align: center; height: 40.375px; width: 22.6986%;"><em>No</em></td>
<td style="text-align: center; height: 40.375px; width: 24.3399%;"><em>No</em></td>
<td style="text-align: center; width: 22.9489%; height: 40.375px;"><em>No</em></td>
</tr>
</tbody>
</table>
<h3><img style="font-size: 16px;" src="https://github.com/user-attachments/assets/59f6956a-44d1-465e-8b00-38225c27031a" alt="" width="200" height="127" /><img src="https://github.com/user-attachments/assets/393ee691-a30c-43c1-ade0-55d67b245691" alt="" width="200" height="127" /><img style="font-size: 16px;" src="https://github.com/user-attachments/assets/c3808359-daba-4e72-a576-73e3c71e95d7" alt="" width="200" height="173" /></h3>
<h2>FAQ:</h2>
<table style="border-collapse: collapse; width: 49.1302%; height: 209px; background-color: #95a5a6; border-color: #000000; border-style: solid;" border="2" cellpadding="6">
<tbody>
<tr style="height: 10px;">
<td style="width: 37.7958%; font-weight: bold; height: 70px;">&nbsp;Can I translate the mod into another language?</td>
<td style="width: 67.743%; height: 70px; text-align: center;">The mod is currently available in <strong>English</strong> and <strong>French</strong>. If you want to suggest another translation, you can submit it via GitHub using the link in the &ldquo;Source&rdquo; tab.</td>
</tr>
<tr style="height: 38px;">
<td style="width: 37.7958%; font-weight: bold; height: 67px;">&nbsp;I can&rsquo;t pour molten copper into the hoop mold!</td>
<td style="width: 67.743%; height: 67px; text-align: center;">That is normal... copper hoops do not exist in the base game :)</td>
</tr>
<tr style="height: 72px;">
<td style="width: 37.7958%; font-weight: bold; height: 72px;">Why doesn't pouring steel into the molds work?</td>
<td style="width: 67.743%; text-align: center; height: 72px;">
<p>Steel can't be poured into molds to make tools or parts like copper can. You need to follow the steelmaking process first, and then forge the item on an anvil.</p>
</td>
</tr>
</tbody>
</table>
<h3>Future plans:</h3>
<ul style="list-style-type: circle;">
<li>Add support for existing configuration menu mods.</li>
<li>Add new translation languages.</li>
<li>Improve animations.</li>
<li>Add an in-game guide.</li>
<li><span style="text-decoration: line-through;">Add metal rod heavymold.</span></li>
<li><span style="text-decoration: line-through;">Optimized the existing C# classes and made them JSON-driven </span>to support addon creation.</li>
</ul>
<h3>Incompatibility:</h3>
<ul style="list-style-type: circle;">
<li><strong>Config menu mods</strong> : the config file cannot currently be edited through in-game config menu mods. Manual editing of <strong><code>VintagestoryData/ModConfig/vanillamoremolds.json</code></strong> is required. Support is planned for a future update.</li>
<li><span style="color: #e74c3c;"><strong>Save files from 1.x</strong> : Molds from version of VanillaMoreMolds 1.x will be broken starting from 2.0. It is recommended to remove all Vanilla More Molds molds from your world before updating.</span></li>
</ul>
<h3>Know issue:</h3>
<ul style="list-style-type: circle;">
<li>If you break Heavy Molds by hand instead of retrieving them properly, the sand does not display the correct texture. <em>(This is a known bug the mold should appear empty, i'm gona fix this in the coming days.)</em></li>
</ul>
<h3>Update:</h3>
<p>22/07/2026 : <strong>2.1.2</strong> : Fixed several issues in the translation files, added a Russian translation by <strong>ChimMAG</strong>.</p>
<p>17/07/2026 : <strong>2.1.1</strong> : Various fixes and added a Chinese translation, contributed by <strong>ch4Ver</strong> (CH44).</p>
<p>07/07/2026 : <strong>2.1.0</strong> : New rod HeavyMold, Heavymold 3d model tweek, bug fix and more.</p>
<p>02/07/2026 : <strong>2.0.1</strong> : HotFix : Fixed config issue, and VanillaMoreMoldsModSystem.cs. Add new modicon.png.</p>
<p>01/07/2026 : <strong>2.0.0</strong> : Major overhaul of the mod, introduction of heavy molds, addition of the config system.</p>
<p>19/02/2025 : <strong>1.1.2</strong> : Community Update.</p>
<p>21/01/2025 : <strong>1.1.1</strong> : Fixed mod loading issue in 1.1.0.</p>
<p>18/01/2025 : <strong>1.1.0</strong> : Full support for 1.20.0, model tweaks, and preparation for future updates.</p>
<p>08/01/2025 : <strong>1.0.0</strong> : Mod Release.</p>
<p>&nbsp;</p>
<p><img src="https://github.com/user-attachments/assets/2bb93271-edba-4f36-b0fb-d805279a8363" alt="" width="178" height="178" /></p>
