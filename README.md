<hr />
<p>&nbsp;</p>
<p><img style="display: block; margin-left: auto; margin-right: auto;" src="https://github.com/user-attachments/assets/614be4e9-8aa2-4b80-8717-54ce0c128c68" alt="" width="777" height="437" /></p>
<h4 style="text-align: center;"><em>Do you want more diverse molds while staying true to the spirit and mechanics of the game? This mod is for you!</em></h4>
<h1 style="text-align: center;"><span style="color: #e74c3c;"><strong>The VanillaMoreMolds update has finally arrived!</strong><br /><strong>Welcome to Version 2.0!</strong><br /><br />Thank you for the 13,000+ downloads! </span></h1>
<p>&nbsp;</p>
<h4 style="text-align: center;"><strong>The VanillaMoreMolds project aims to add new varieties of molds to expand the arsenal of your forge, while staying true to the base game mechanics, smelting logic, and crafting style.</strong></h4>
<h4 style="text-align: center;"><strong>The mod also introduces a brand-new mechanic for certain molds, designed to stay as close as possible to what the base game could naturally offer.</strong></h4>
<p>&nbsp;</p>
<p><span style="font-size: 14pt;">Hey everyone!</span></p>
<p><span style="font-size: 14pt;">It's been a while... yeah, more than a year. I have to admit, the introduction of color variants during the development of 2.0 really hit my motivation. </span></p>
<p><span style="font-size: 14pt;">It wasn't anything impossible to overcome, but it gave me the opportunity to completely rethink and rewrite my code from the ground up. Anyway, that's all behind me now! I'm incredibly happy to finally present this new version to you, and I really hope you'll enjoy it, especially the new Heavy Molds!</span></p>
<p><span style="font-size: 14pt;">I have to admit, the new Heavy Mold mechanic is what took up most of my time. Between digging through the API, reading the game's source code on GitHub, and getting help from a few people who reached out, I finally managed to make it work! <em>( Thx </em><a class="mention username mceNonEditable" href="https://mods.vintagestory.at/show/user/4FB78B639957E9214D82" data-user-hash="4FB78B639957E9214D82">Laerinok</a><em> for Net10 solution)</em></span></p>
<p><span style="font-size: 14pt;">That said, I still think there's plenty of room for optimization in my classes, but I'm really happy with how everything turned out.</span></p>
<p><span style="font-size: 14pt;">As always, don't hesitate to share your feedback. If you run into any bugs, please send me your console log as well. I'll be keeping a close eye on everything over the next few days so I can fix any issues you might find as quickly as possible.</span></p>
<h2>What&rsquo;s new in 2.0?</h2>
<p>&nbsp;</p>
<ul style="list-style-type: circle;">
<li><strong>Tool mold color variants :</strong> tool molds are now available in all clay colors.</li>
<li><strong>Removal of large ingot molds :</strong> the old Light, Medium, and Heavy Ingot Molds have been removed and replaced by the new heavy mold system.</li>
<li><strong>Heavy molds :</strong> new large two-handed molds supporting two output variants: ingots (x6) or metal plates (x4).</li>
<li><strong>Config file :</strong> a versioned <strong><code>vanillamoremolds.json</code></strong> config is generated in <strong><code>VintagestoryData/ModConfig/</code></strong>. Each mold can be individually enabled or disabled.</li>
<li><strong>Improved interaction help bubbles :</strong> contextual hints are now shown depending on the mold stage (sand fill, ingot, plate).</li>
<li><strong>Optimisation :</strong> cleaner architecture and non-visible faces of the 3D mold models are now disabled to reduce rendering overhead.</li>
<li><strong>Fixed rotation of special molds :</strong> special mold variants now display at the correct angle when placed.</li>
<li><strong>Fixed hitbox of special molds :</strong> collision and selection boxes for special molds are now accurate.</li>
</ul>
<h2 style="text-align: left;">Feature: Tool molds</h2>
<table style="border-collapse: collapse; width: 75.6267%; height: 106px; background-color: #95a5a6; border-color: #000000; border-style: solid;" border="3">
<tbody>
<tr style="height: 21px;">
<td style="width: 11.1111%; text-align: left; height: 21px;"><strong>Mold name:</strong></td>
<td style="width: 11.1111%; text-align: center; height: 21px;"><strong>Sawblade Mold</strong></td>
<td style="width: 11.1111%; text-align: center; height: 21px;"><strong>Nail and Strips Mold</strong></td>
<td style="width: 11.1111%; text-align: center; height: 21px;"><strong>Arrowhead Mold</strong></td>
<td style="width: 11.1111%; text-align: center; height: 21px;"><strong>Hoop Mold</strong></td>
<td style="width: 11.1111%; text-align: center; height: 21px;"><strong>Plate Mold</strong></td>
<td style="width: 11.1111%; text-align: center; height: 21px;"><strong>Spearhead Mold</strong></td>
<td style="width: 11.1111%; text-align: center; height: 21px;"><strong>Knifeblade Mold</strong></td>
<td style="width: 11.1111%; text-align: center; height: 21px;"><strong>Scythehead Mold</strong></td>
</tr>
<tr style="height: 20px;">
<td style="text-align: left; height: 20px;"><strong>Molten metal required:</strong></td>
<td style="text-align: center; height: 20px;">100</td>
<td style="text-align: center; height: 20px;">75</td>
<td style="text-align: center; height: 20px;">100</td>
<td style="text-align: center; height: 20px;">100</td>
<td style="text-align: center; height: 20px;">200</td>
<td style="text-align: center; height: 20px;">100</td>
<td style="text-align: center; height: 20px;">100</td>
<td style="text-align: center; height: 20px;">100</td>
</tr>
<tr style="height: 20px;">
<td style="text-align: left; height: 20px;"><strong>Drop:</strong></td>
<td style="text-align: center; height: 20px;">x1 sawblade</td>
<td style="text-align: center; height: 20px;">x3 metal nails and strips</td>
<td style="text-align: center; height: 20px;">x8 arrowheads</td>
<td style="text-align: center; height: 20px;">x1 hoop</td>
<td style="text-align: center; height: 20px;">x1 metal plate</td>
<td style="text-align: center; height: 20px;">x1 spearhead</td>
<td style="text-align: center; height: 20px;">x1 knifeblade</td>
<td style="text-align: center; height: 20px;">x1 scythehead</td>
</tr>
<tr style="height: 21px;">
<td style="text-align: left; height: 21px;"><strong>Type of clay:</strong></td>
<td style="text-align: center; height: 21px;"><em>Blue, Fire, Red</em></td>
<td style="text-align: center; height: 21px;"><em>Blue, Fire, Red</em></td>
<td style="text-align: center; height: 21px;"><em>Blue, Fire, Red</em></td>
<td style="text-align: center; height: 21px;"><em>Blue, Fire, Red</em></td>
<td style="text-align: center; height: 21px;"><em>Blue, Fire, Red</em></td>
<td style="text-align: center; height: 21px;"><em>Blue, Fire, Red</em></td>
<td style="text-align: center; height: 21px;"><em>Blue, Fire, Red</em></td>
<td style="text-align: center; height: 21px;"><em>Blue, Fire, Red</em></td>
</tr>
<tr style="height: 21px;">
<td style="text-align: left; height: 21px;"><strong>Active by default:</strong></td>
<td style="text-align: center; height: 21px;"><em>✗ Disabled</em></td>
<td style="text-align: center; height: 21px;"><em>✓ Enabled</em></td>
<td style="text-align: center; height: 21px;"><em>✗ Disabled</em></td>
<td style="text-align: center; height: 21px;"><em>✓ Enabled</em></td>
<td style="text-align: center; height: 21px;"><em>✓ Enabled</em></td>
<td style="text-align: center; height: 21px;"><em>✗ Disabled</em></td>
<td style="text-align: center; height: 21px;"><em>✗ Disabled</em></td>
<td style="text-align: center; height: 21px;"><em>✗ Disabled</em></td>
</tr>
</tbody>
</table>
<h2 style="text-align: left;">Feature: Heavy molds</h2>
<p><span style="color: #e74c3c;"><em><strong>⚠ Note for players upgrading from 1.x:</strong> The old Light, Medium, and Heavy Ingot Molds have been removed and replaced by the new Heavy Mold system introduced in 2.0. If you had large ingot molds in your world, they will no longer be valid. The new Heavy Molds offer the same mass-production purpose with a richer crafting process and two output variants (ingots or plates).</em></span></p>
<p><strong>Heavy molds are large two-handed molds built in two steps :</strong> first shape the Heavy Base Mold via <strong>clayforming</strong>, fire it in a kiln, then assemble it with <strong>rope</strong>, metal <strong>nails &amp; strips</strong>, and <strong>sticks</strong>&nbsp;to create the placeable Heavy Mold. Once placed, fill it with <strong>sand</strong>, then Shift + right-click with an <strong>ingot</strong> or a <strong>metal plate</strong> to choose the output variant before pouring the metal.</p>
<p>&nbsp;</p>
<table style="border-collapse: collapse; width: 29.1121%; background-color: #95a5a6; border-color: #000000; border-style: solid; height: 111px;" border="3" cellpadding="1">
<tbody>
<tr style="height: 21px;">
<td style="width: 30%; text-align: left; height: 21px;"><strong>Variant:</strong></td>
<td style="width: 35%; text-align: center; height: 21px;"><strong>Ingot Mold</strong></td>
<td style="width: 35%; text-align: center; height: 21px;"><strong>Plate Mold</strong></td>
</tr>
<tr style="height: 20px;">
<td style="text-align: left; height: 20px;"><strong>Molten metal required:</strong></td>
<td style="text-align: center; height: 20px;">600</td>
<td style="text-align: center; height: 20px;">800</td>
</tr>
<tr style="height: 20px;">
<td style="text-align: left; height: 20px;"><strong>Drop:</strong></td>
<td style="text-align: center; height: 20px;">x6 Ingots</td>
<td style="text-align: center; height: 20px;">x4 Metal plates</td>
</tr>
<tr style="height: 21px;">
<td style="text-align: left; height: 21px;"><strong>Base clay (clayforming):</strong></td>
<td style="text-align: center; height: 21px;"><em>Blue, Fire, Red</em></td>
<td style="text-align: center; height: 21px;"><em>Blue, Fire, Red</em></td>
</tr>
<tr style="height: 21px;">
<td style="text-align: left; height: 21px;"><strong>Active by default:</strong></td>
<td style="text-align: center; height: 21px;"><em>✓ Enabled</em></td>
<td style="text-align: center; height: 21px;"><em>✓ Enabled</em></td>
</tr>
</tbody>
</table>
<h3><img style="font-size: 16px;" src="https://github.com/user-attachments/assets/59f6956a-44d1-465e-8b00-38225c27031a" alt="" width="200" height="127" /><img src="https://github.com/user-attachments/assets/393ee691-a30c-43c1-ade0-55d67b245691" alt="" width="200" height="127" />&nbsp; &nbsp;<img style="font-size: 16px;" src="https://github.com/user-attachments/assets/c3808359-daba-4e72-a576-73e3c71e95d7" alt="" width="200" height="173" /></h3>
<h2>FAQ:</h2>
<p>&nbsp;</p>
<table style="border-collapse: collapse; width: 49.5233%; height: 116px; background-color: #95a5a6; border-color: #000000; border-style: solid;" border="2" cellpadding="6">
<tbody>
<tr style="height: 10px;">
<td style="width: 37.7958%; font-weight: bold; height: 10px;">&nbsp;Can I translate the mod into another language?</td>
<td style="width: 67.743%; height: 10px; text-align: center;">The mod is currently available in <strong>English</strong> and <strong>French</strong>. If you want to suggest another translation, you can submit it via GitHub using the link in the &ldquo;Source&rdquo; tab.</td>
</tr>
<tr style="height: 38px;">
<td style="width: 37.7958%; font-weight: bold; height: 38px;">&nbsp;I can&rsquo;t pour molten copper into the hoop mold!</td>
<td style="width: 67.743%; height: 38px; text-align: center;">That is normal... copper hoops do not exist in the base game :)</td>
</tr>
</tbody>
</table>
<h3>Future plans:</h3>
<ul style="list-style-type: circle;">
<li>Add support for existing configuration menu mods.</li>
<li>Add new translation languages.</li>
<li>Improve animations.</li>
<li>Add an in-game guide.</li>
</ul>
<h3>Incompatibility:</h3>
<ul style="list-style-type: circle;">
<li><strong>Config menu mods</strong> : the config file cannot currently be edited through in-game config menu mods. Manual editing of <strong><code>VintagestoryData/ModConfig/vanillamoremolds.json</code></strong> is required. Support is planned for a future update.</li>
<li><span style="color: #e74c3c;"><strong>Save files from 1.x</strong> : Molds from version of VanillaMoreMolds 1.x will be broken starting from 2.0. It is recommended to remove all Vanilla More Molds molds from your world before updating.</span></li>
</ul>
<h3>Update:</h3>
<p>02/08/2026 : <strong>2.0.1</strong> : HotFix : Fixed config issue, and VanillaMoreMoldsModSystem.cs. Add new modicon.png</p>
<p>01/07/2026 : <strong>2.0.0</strong> : Major overhaul of the mod, introduction of heavy molds, addition of the config system</p>
<p>19/02/2025 : <strong>1.1.2</strong> : Community Update</p>
<p>21/01/2025 : <strong>1.1.1</strong> : Fixed mod loading issue in 1.1.0</p>
<p>18/01/2025 : <strong>1.1.0</strong> : Full support for 1.20.0, model tweaks, and preparation for future updates</p>
<p>08/01/2025 : <strong>1.0.0</strong> : Mod Release</p>
<p>&nbsp;</p>
<p><img src="https://github.com/user-attachments/assets/2bb93271-edba-4f36-b0fb-d805279a8363" alt="" width="178" height="178" /></p>
